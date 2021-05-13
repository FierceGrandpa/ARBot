using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class BotMenu
    {
        private readonly ImmutableHashSet<Type> _availableMenus;
        private readonly ITelegramBotClient _client;

        public BotMenu(ITelegramBotClient client)
        {
            _client = client;

            // TODO: good?
            _availableMenus = new HashSet<Type>
            {
                // Activities
                typeof(GetActivityMenu),
                typeof(SelectCategoryMenu),
                // Settings
                typeof(SendingTimeModeMenu),
                // Change Menu
                typeof(SelectMenu),
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
            if (_availableMenus.Contains(menuType))
                return (MenuItem)Activator.CreateInstance(menuType, arguments);

            throw new UnsupportedMenuItem(menuType.ToString());
        }
    }
}