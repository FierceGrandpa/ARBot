using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class ApplyMenu : MenuItem
    {
        public ApplyMenu(IReadOnlyList<string> arguments)
        {
            Description = "Чтобы применить настройки, нажмите *Применить*, в противном случае нажмите на *Отменить*";
            Command = $"set:{arguments[0]}";
        }

        protected override void GenerateButtons() =>
            Buttons.Add(new List<InlineKeyboardButton>
            {
                new() {Text = "✅ Применить", SwitchInlineQuery = Command},
                new() {Text = "❌ Отменить",  CallbackData = $"switch {typeof(MainMenu)}"}
            });
    }
}