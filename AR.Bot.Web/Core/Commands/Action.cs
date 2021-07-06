using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Core.Menu;
using AR.Bot.Core.Services;
using AR.Bot.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AR.Bot.Core.Commands
{
    public class ActionCommand : ICommand
    {
        public string Name { get; }
        public bool IsRequiredArgs { get; }

        private readonly BotMenu _botMenu;
        private readonly ITelegramBotClient _client;
        private readonly IActivityService _activityService;
        private readonly ITelegramUserRepository _userRepository;

        public ActionCommand(string name, bool isRequiredArgs,
            BotMenu botMenu,
            ITelegramBotClient client,
            IActivityService activityService,
            ITelegramUserRepository userRepository)
        {
            _botMenu = botMenu;
            _client = client;
            _activityService = activityService;
            _userRepository = userRepository;

            Name = name;
            IsRequiredArgs = isRequiredArgs;
        }

        public async Task<CommandExecutionResult> Execute(CallbackQuery callbackQuery, params string[] args)
        {
            switch (args.First())
            {
                case "getRandomActivity":
                    var user = await _userRepository.GetOrCreate(callbackQuery.Message.Chat.Id);
                    try
                    {
                        var activity = await _activityService.GetRandomActivity(user.Id);
                        var activityName = $"Активность: <b>{activity.Title}</b>\n";
                        var description = $"\n{activity.Description}\n\n";
                        var age = $"Возраст: {activity.MinAge}-{activity.MaxAge}\n";
                        var skills =
                            $"Развивает: {string.Join(',', activity.Skills.Select(e => e.Title))}\n"; // TODO: Ext

                        await _client.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                            $"{activityName}{age}{skills}{description}",
                            replyToMessageId: callbackQuery.Message.MessageId,
                            parseMode: ParseMode.Html);
                    }
                    catch (RandomActivityException)
                    {
                        await _client.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                            "<b>Активности на сегодня закончились :(\nПриходите завтра</b>",
                            replyToMessageId: callbackQuery.Message.MessageId);
                    }

                    break;
            }
            return CommandExecutionResult.DefaultResult;
        }
    }
}