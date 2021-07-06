using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Core.Menu;
using AR.Bot.Core.Services;
using AR.Bot.Repositories;
using AR.Bot.Web.Core;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AR.Bot.Core.Commands
{
    public class SwitchCommand : ICommand
    {
        public string Name { get; }
        public bool IsRequiredArgs { get; }

        private readonly BotMenu _botMenu;
        private readonly ITelegramBotClient _client;
        private readonly IActivityService _activityService;
        private readonly ITelegramUserRepository _userRepository;

        public SwitchCommand(string name, bool isRequiredArgs,
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
            if (IsRequiredArgs && args == null) throw new InvalidOperationException();
            
            var menuType = Type.GetType(args[0]);
            await _botMenu.SwitchMenu(menuType, args.Skip(1).ToArray(),
                callbackQuery.Message.Chat.Id,
                callbackQuery.Message.MessageId);
            return CommandExecutionResult.DefaultResult;
        }
    }
}