using System;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public abstract class MenuItem
    {
        protected readonly List<IEnumerable<InlineKeyboardButton>> Buttons = new();

        public string ItemTitle; // TODO: Remove crutch...
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

        public InlineKeyboardMarkup GenerateMarkup()
        {
            GenerateButtons();
            return new InlineKeyboardMarkup(Buttons);
        }
    }
}