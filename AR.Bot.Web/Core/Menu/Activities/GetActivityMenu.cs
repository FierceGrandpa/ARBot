using System;
using System.Collections.Generic;
using System.Linq;
using AR.Bot.Core.Services;
using AR.Bot.Domain;
using AR.Bot.Repositories;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class GetActivityMenu : MenuItem
    {
        public GetActivityMenu(IReadOnlyList<string> arguments)
        {
            ItemTitle = "Получить активность!";
            Description = "*Вы можете получить рандомную активность или выбрать категорию, по которой вам выдадут рандомную активность*";

            Command = nameof(Setting.MailingMode).ToLowerInvariant();
        }

        protected override void GenerateButtons()
        {
            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "Получить рандомную активность!",
                    CallbackData = "action getRandomActivity"
                }
            });

            var item = new SelectCategoryMenu(null, null);
            if (item == null)
                throw new ArgumentNullException(nameof(SelectCategoryMenu));

            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = item.ItemTitle,
                    CallbackData = $"switch {item.GetType()}"
                }
            });

            base.GenerateButtons();
        }
    }
}