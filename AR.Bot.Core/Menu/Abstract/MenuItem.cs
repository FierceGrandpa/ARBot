using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public abstract class MenuItem
    {
        protected readonly List<IEnumerable<InlineKeyboardButton>> Buttons = new();

        protected string ItemTitle;
        protected string Command;
      
        public string Description;

        protected virtual void GenerateButtons() =>
            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "❌ Назад", 
                    CallbackData = $"switch {typeof(MainMenu)}"
                }
            });

        protected void GenerateButtons(IEnumerable<Type> menuItems)
        {
            foreach (var menuItem in menuItems)
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

        public InlineKeyboardMarkup GenerateMarkup()
        {
            GenerateButtons();
            return new InlineKeyboardMarkup(Buttons);
        }
    }
}