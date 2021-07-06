namespace AR.Bot.Core.Commands
{
    public class CommandExecutionResult
    {
        public CommandExecutionStatus Status { get; private set; }

        public string Message { get; private set; }

        public static CommandExecutionResult DefaultResult =>
            new CommandExecutionResult
            {
                Status = CommandExecutionStatus.Ok,
                Message = "Ok"
            };
    }
}