using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Core.Menu;
using Telegram.Bot;
using Telegram.Bot.Types;

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

        // TODO: Good
        // Now: Key is command, value means is command require arguments
        private readonly ImmutableDictionary<string, bool> _supportedCommands;

        public CallbackQueryHandler(BotMenu botMenu, ITelegramBotClient client)
        {
            _botMenu = botMenu;
            _client  = client;

            _supportedCommands = new Dictionary<string, bool>
            {
                {"switch", true},
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
            }

            await _client.AnswerCallbackQueryAsync(callbackQuery.Id);
        }
    }
}