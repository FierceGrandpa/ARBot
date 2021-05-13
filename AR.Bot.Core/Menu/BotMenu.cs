using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using AR.Bot.Domain;
using AR.Bot.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class BotMenu
    {
        private readonly ImmutableHashSet<Type> _availableMenus;
        private readonly ITelegramBotClient _client;
        private readonly IRepository<Category> _categoryRepository;

        public BotMenu(ITelegramBotClient client, IRepository<Category> categoryRepository)
        {
            _client = client;
            _categoryRepository = categoryRepository;

            // TODO: good?
            _availableMenus = new HashSet<Type>
            {
                // Activities
                typeof(GetActivityMenu),
                // Settings
                typeof(SendingTimeModeMenu),
                // Change Menu
                typeof(ApplyMenu),
                // MainMenu
                typeof(MainMenu),

            }.ToImmutableHashSet();
        }

        public async Task SendMainMenu(long chatId)
        {
            var menu = new MainMenu(null);
            await _client.SendTextMessageAsync(chatId, menu.Description,
                ParseMode.Markdown,
                replyMarkup: menu.GenerateMarkup());
        }

        public async Task SwitchMenu(Type menuType, string[] arguments, long chatId, int messageId)
        {
            var item = GetMenuItem(menuType, arguments);
            await _client.EditMessageTextAsync(chatId, messageId, item.Description,
                ParseMode.Markdown,
                replyMarkup: item.GenerateMarkup());
        }

        private MenuItem GetMenuItem(Type menuType, IEnumerable arguments)
        {
            // TODO: Remove crutch...
            if (_availableMenus.Contains(menuType))
                return (MenuItem)Activator.CreateInstance(menuType, arguments);

            throw new UnsupportedMenuItem(menuType.ToString());
        }
    }
}