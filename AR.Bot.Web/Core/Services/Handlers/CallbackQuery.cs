using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Core.Commands;
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
        private readonly ImmutableList<ICommand> _supportedCommands;

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

            var switchCommand = new SwitchCommand(
                "switch", true, botMenu, client, activityService, userRepository);
            var actionCommand = new ActionCommand(
                "action", true, botMenu, client, activityService, userRepository);
            
            _supportedCommands = new List<ICommand>
            {
                switchCommand, actionCommand
            }.ToImmutableList();
        }

        public async Task HandleCallbackQueryAsync(CallbackQuery callbackQuery)
        {
            var data = callbackQuery.Data;
            if (string.IsNullOrWhiteSpace(data))
                return;

            // [0] - command; [1] - arguments
            var parts = data.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var commandName = parts[0];
            var args = parts.Length > 1 ? parts[1].Split('#') : null; // TODO: Good

            try
            {
                var command = _supportedCommands.First(c => c.Name == commandName);
                await command.Execute(callbackQuery, args);
            }
            catch (InvalidOperationException)
            {
                throw new UnsupportedCommand(callbackQuery.Data);
            }

            await _client.AnswerCallbackQueryAsync(callbackQuery.Id);
        }
    }
}