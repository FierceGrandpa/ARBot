using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace AR.Bot
{
    public static class TelegramExtensions
    {
        public static async Task<bool> IsAdministrator(this User user, long chatId, TelegramBotClient client)
        {
            var chatAdmins = await client.GetChatAdministratorsAsync(chatId);

            // TODO: Make better when TG lib will update
            return chatAdmins.Any(x => x.User.Id == user.Id); // ¯\_(ツ)_/¯
        }

        public static bool IsCommand(this Message message) => 
            message.Entities?.Length == 1 && 
            message.Entities[0].Type == MessageEntityType.BotCommand;
    }
}