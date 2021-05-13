using System;
using System.Collections.Generic;
using AR.Bot.Domain;
using AR.Bot.Repositories;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class MainMenu : MenuItem
    {
        private readonly List<Type> _mainMenuItems;

        // TODO: Remove crutch...
        public MainMenu(IReadOnlyList<string> arguments)
        {
            Description = "Вы можете настроить время рассылки или получить активность";
            ItemTitle = "Главное Меню";
            
            _mainMenuItems = new List<Type>
            {
                // typeof(SelectCategoryMenu), // TODO: Remove crutch...
                typeof(SendingTimeModeMenu),
                typeof(GetActivityMenu)
            };
        }

        protected override void GenerateButtons()
        {
            // TODO: Remove crutch...
            foreach (var menuItem in _mainMenuItems)
            {
                var item = (MenuItem)Activator.CreateInstance(menuItem, new object[] { null });
                // TODO: Exception
                if (item != null)
                {
                    Buttons.Add(new List<InlineKeyboardButton>
                    {
                        new()
                        {
                            Text = item.ItemTitle,
                            CallbackData = $"switch {item.GetType()}"
                        }
                    });
                }
            }
        }
    }
}