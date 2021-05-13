using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    internal class SelectMenu : MenuItem
    {
        public SelectMenu(IReadOnlyList<string> arguments)
        {
            Description = "Чтобы получить активность нажмите *Применить*, чтобы вернуться назад, нажмите на *Назад*";
            Command = $"select:{arguments[0]}";
        }

        protected override void GenerateButtons() =>
            Buttons.Add(new List<InlineKeyboardButton>
            {
                new() {Text = "✅ Получить", SwitchInlineQuery = Command},
                new() {Text = "❌ Назад",  CallbackData = $"switch {typeof(SelectCategoryMenu)}"}
            });
    }
}