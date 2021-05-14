using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AR.Bot.Web.Validation
{
    // TODO: Add Blacklist
    public class MessageValidator
    {
        public bool GroupMessageValid(Message message) =>
            // !_blacklist.GroupIdsBlacklist.Contains(message.Chat.Id) TODO
            message.Type == MessageType.Text && 
            message.Text != null
            // message.Text.Length <= _charLimit TODO: We Need?
            // !_blacklist.TextsBlacklist.Contains(message.Text.ToLowerInvariant()) TODO
            ;
    }
}