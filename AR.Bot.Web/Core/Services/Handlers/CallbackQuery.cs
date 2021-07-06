using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Core.Menu;
using AR.Bot.Domain;
using AR.Bot.Repositories;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Services
{
    public interface ICallbackQueryHandler
    {
        Task HandleCallbackQueryAsync(CallbackQuery callbackQuery);
    }

    public class CallbackQueryHandler : ICallbackQueryHandler
    {
        private readonly BotMenu _botMenu;
        private readonly ITelegramBotClient _client;
        private readonly IActivityService _activityService;
        private readonly ITelegramUserRepository _userRepository;

        // TODO: Good
        // Now: Key is command, value means is command require arguments
        private readonly ImmutableDictionary<string, bool> _supportedCommands;

        public CallbackQueryHandler(
            BotMenu botMenu,
            ITelegramBotClient client,
            IActivityService activityService,
            ITelegramUserRepository userRepository)
        {
            _botMenu = botMenu;
            _client = client;

            _activityService = activityService;

            _userRepository = userRepository;

            _supportedCommands = new Dictionary<string, bool>
            {
                {"switch", true},
                {"action", true},
            }.ToImmutableDictionary();
        }

        public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery)
        {
            var data = callbackQuery.Data;
            if (string.IsNullOrWhiteSpace(data))
                return;

            // [0] - command; [1] - arguments
            var parts = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var command = parts[0];
            var args = parts.Length > 1 ? parts[1].Split('#') : null; // TODO: Good

            // TODO: Refactor (Make Class and etc...)
            var isSupportedCommand = _supportedCommands.TryGetValue(command, out var isRequiredArgs);
            var isSupportedArgs = isRequiredArgs == (args != null);

            if (!isSupportedCommand || !isSupportedArgs)
                throw new UnsupportedCommand(callbackQuery.Data);

            switch (command)
            {
                case "switch" when args != null:
                    var menuType = Type.GetType(args[0]);
                    await _botMenu.SwitchMenu(menuType, args.Skip(1).ToArray(),
                        callbackQuery.Message.Chat.Id,
                        callbackQuery.Message.MessageId);
                    break;
                case "action" when args != null: // TODO: good?
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
                            
                            /*
                            var activity = await _activityService.GetRandomActivity(user.Id);
                            if (activity == null)
                            {
                                await _client.SendTextMessageAsync(callbackQuery.Message.Chat.Id, "<b>Активности на сегодня закончились :(\nПриходите завтра</b>", replyToMessageId: callbackQuery.Message.MessageId);
                            }
                            else
                            {
                                var activityName = $"Активность: <b>{activity.Title}</b>\n";
                                var description = $"\n{activity.Description}\n\n";
                                var age = $"Возраст: {activity.MinAge}-{activity.MaxAge}\n";
                                var skills = $"Развивает: {string.Join(',', activity.Skills.Select(e => e.Title))}\n"; // TODO: Ext

                                await _client.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                                    $"{activityName}{age}{skills}{description}",
                                    replyToMessageId: callbackQuery.Message.MessageId,
                                    parseMode: ParseMode.Html);
                            }*/

                            break;
                    }

                    break;
            }

            await _client.AnswerCallbackQueryAsync(callbackQuery.Id);
        }
    }
}