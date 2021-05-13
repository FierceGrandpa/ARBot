using System.Collections.Generic;
using AR.Bot.Core.Services;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Menu
{
    public class SendingTimeModeMenu : MenuItem
    {
        public SendingTimeModeMenu(IReadOnlyList<string> arguments)
        {
            ItemTitle = "Изменить время рассылки";
            Description = "*Здесь вы можете изменить время рассылки* \n\n" +
                          "*По умолчанию* — 12:00 (По умолчанию) \n" +
                          "*Утро* — 9:00 \n" +
                          "*Обед* — 15:00 \n" +
                          "*Вечер* — 19:00 \n" +
                          "*Ваше время* — произвольное время";

            Command = nameof(Setting.MailingMode).ToLowerInvariant();
        }

        protected override void GenerateButtons()
        {
            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "По умолчанию (12:00)",
                    CallbackData = $"switch {typeof(ApplyMenu)}#{Command}=default"
                }
            });

            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "Утро (9:00)", 
                    CallbackData = $"switch {typeof(ApplyMenu)}#{Command}=morning"
                }
            });

            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "Обед (15:00)",
                    CallbackData = $"switch {typeof(ApplyMenu)}#{Command}=lunch"
                }
            });

            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "Вечер (19:00)",
                    CallbackData = $"switch {typeof(ApplyMenu)}#{Command}=evening"
                }
            });

            Buttons.Add(new List<InlineKeyboardButton>
            {
                new()
                {
                    Text = "Свое время",
                    CallbackData = $"switch {typeof(ApplyMenu)}#{Command}=custom"
                }
            });

            base.GenerateButtons();
        }
    }
}