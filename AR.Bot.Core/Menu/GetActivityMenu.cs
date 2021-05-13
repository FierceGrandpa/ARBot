using System.Collections.Generic;
using AR.Bot.Core.Services;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class GetActivityMenu : MenuItem
    {
        public GetActivityMenu(IReadOnlyList<string> arguments)
        {
            ItemTitle = "Получить активность!";
            Description = "*Вы можете получить рандомную активность*";

            Command = nameof(Setting.MailingMode).ToLowerInvariant();
        }

        protected override void GenerateButtons()
        {
            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "Получить рандомную активность!",
                    CallbackData = "action:getRandomActivity"
                }
            });

            base.GenerateButtons();
        }
    }
}