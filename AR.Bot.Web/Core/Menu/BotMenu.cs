using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
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

        private readonly IRepository<Category> _categoriesRepository;
        public BotMenu(ITelegramBotClient client, IRepository<Category> categoriesRepository)
        {
            _client = client;

            _categoriesRepository = categoriesRepository ?? throw new ArgumentNullException(nameof(categoriesRepository));

            // TODO: good?
            _availableMenus = new HashSet<Type>
            {
                // Activities
                typeof(SelectCategoryMenu),
                typeof(GetActivityMenu),
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
            // TODO: Remove crutch...
            if (_availableMenus.Contains(menuType))
            {
                if (menuType == typeof(SelectCategoryMenu))
                {
                    return new SelectCategoryMenu((IReadOnlyList<string>)arguments, _categoriesRepository);
                }

                if (menuType == typeof(GetActivityMenu))
                {
                    return new GetActivityMenu((IReadOnlyList<string>)arguments);
                }

                if (menuType == typeof(SendingTimeModeMenu))
                {
                    return new SendingTimeModeMenu((IReadOnlyList<string>)arguments);
                }

                if (menuType == typeof(ApplyMenu))
                {
                    return new ApplyMenu((IReadOnlyList<string>)arguments);
                }

                if (menuType == typeof(SelectMenu))
                {
                    return new ApplyMenu((IReadOnlyList<string>)arguments);
                }

                if (menuType == typeof(MainMenu))
                {
                    return new MainMenu((IReadOnlyList<string>)arguments);
                }
            }
               

            throw new UnsupportedMenuItem(menuType.ToString());
        }
    }
}