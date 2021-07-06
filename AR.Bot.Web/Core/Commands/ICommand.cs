using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AR.Bot.Web.Core;
using Telegram.Bot.Types;

namespace AR.Bot.Core.Commands
{
    public interface ICommand
    {
        string Name { get; }
        bool IsRequiredArgs { get; }
        Task<CommandExecutionResult> Execute(CallbackQuery callbackQuery, params string[] args);
    }
}