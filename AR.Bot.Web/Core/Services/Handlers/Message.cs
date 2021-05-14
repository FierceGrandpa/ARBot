using System;
using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Core.Menu;
using AR.Bot.Domain;
using AR.Bot.Repositories;
using AR.Bot.Web.Validation;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Services
{
    public interface IMessageHandler
    {
        Task HandleMessageAsync(Message message);
    }

    public class MessageHandler : IMessageHandler
    {
        private readonly BotMenu _botMenu;

        private readonly string _botUsername;
        private readonly ITelegramBotClient _client;
        private readonly SettingsProcessor _settingsProcessor;
        private readonly MessageValidator _validator;
        private readonly IActivityService _activityService;
        private readonly ITelegramUserRepository _userRepository;

        public MessageHandler(ITelegramBotClient client, BotMenu botMenu,
            MessageValidator validator, 
            SettingsProcessor settingsProcessor, 
            IActivityService activityService,
            ITelegramUserRepository userRepository)
        {
            _client            = client;
            _botMenu           = botMenu;
            _validator         = validator;
            _settingsProcessor = settingsProcessor;

            _activityService = activityService;

            _userRepository = userRepository;

            _botUsername = _client.GetMeAsync().Result.Username;
        }
        
        public async Task HandleMessageAsync(Message message)
        {
            // TODO: Add Metrics
            Log.Information("Got new message {ChatId} | {From} | {ChatType}", message.Chat.Id, message.From, message.Chat.Type);

            // TODO: Add Black List

            switch (message.Chat.Type)
            {
                case ChatType.Group:
                case ChatType.Supergroup:
                    // await HandleGroupMessage(message); TODO: Groups?
                    break;

                case ChatType.Private:
                    await HandlePrivateMessage(message);
                    break;
            }
        }
        
        private async Task HandleGroupMessage(Message message)
        {
            // TODO: Add Metrics
           
            // TODO: Message Valid

            // TODO: Add Black List

            // TODO: Check Is Command Type?

            if (message.Text.StartsWith($"@{_botUsername} set:", StringComparison.InvariantCulture))
            {
                Log.Information("Message by {ChatId} | {From} is a setting changing", message.Chat.Id, message.From);
                await HandleSettingChanging(message);
                return;
            }

            // TODO: 

            switch (await _settingsProcessor.GetMailingMode(message.Chat.Id))
            {
                default:
                    break;
            }
        }

        private async Task HandlePrivateMessage(Message message)
        {
            if (message.Text.StartsWith("select:", StringComparison.InvariantCulture))
            {
                Log.Information("Message by {ChatId} | {From} is a select", message.Chat.Id, message.From);
                await HandleSelect(message);
                return;
            }

            if (message.IsCommand())
            {
                await HandleCommand(message);
                return;
            }

            await _botMenu.SendMainMenu(message.Chat.Id);
        }

        private async Task HandleSelect(Message message)
        {
            var tinyString = message.Text.Replace($"select:", "");
            var splitString = tinyString.Split('=');

            var (param, value) = (splitString[0], splitString[1]);

            switch (param)
            {
                case "categoryId":
                    // TODO: Hmmm, but what if empty?
                    var user = await _userRepository.GetOrCreate(message.Chat.Id);
                    var activity = _activityService.GetRandomActivityByCategory(user.Id, new Guid(value));
                    if (activity == null) // TODO: Rewrite Bad Service
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, "<b>Активности на сегодня закончились :(\nПриходите завтра</b>", replyToMessageId: message.MessageId);
                    }
                    else
                    {
                        await _client.SendTextMessageAsync(message.Chat.Id, "Сделано!", replyToMessageId: message.MessageId);
                    }
                  
                    break;
            }
        }

        private async Task HandleSettingChanging(Message message)
        {
            var tinyString = message.Text.Replace("set:", "");
            var splitString = tinyString.Split('=');

            var (param, value) = (splitString[0], splitString[1]);

            if (!Enum.TryParse(param, true, out Setting setting) || !Enum.IsDefined(typeof(Setting), setting))
                throw new InvalidSettingException();

            if (!_settingsProcessor.ValidateSettings(Enum.Parse<Setting>(param, true), value))
                throw new InvalidSettingValueException();

            switch (setting)
            {
                case Setting.MailingMode:
                    await _settingsProcessor.ChangeMailingMode(message.Chat.Id, Enum.Parse<SendingTimeMode>(value, true));
                    break;
            }

            await _client.SendTextMessageAsync(message.Chat.Id, "Сделано!", replyToMessageId: message.MessageId);
        }

        private async Task HandleCommand(Message message)
        {
            var chatType = message.Chat.Type;
            var command = message.Text[1..];
            string payload = null;

            if (command.Contains("@"))
            {
                var indexOfAt = command.IndexOf('@');
                if (command[(indexOfAt + 1)..] != _botUsername)
                    return;

                command = command[..indexOfAt];
            }

            if (command.Contains(" "))
            {
                var splitCommand = command.Split(" ");

                payload = splitCommand[1];
                command = splitCommand[0];
            }

            // TODO: Refactor

            if (chatType is ChatType.Group or ChatType.Supergroup)
            {
                switch (command)
                {
                    case "settings":
                        // TODO: IsAdministrator?
                        // TODO: Change Settings
                        break;
                    default: return;
                }
            }

            if (chatType is ChatType.Private)
            {
                switch (command)
                {
                    case "start":
                    case "settings": 
                        await _botMenu.SendMainMenu(message.Chat.Id); 
                        break;

                    case "help":
                        // TODO: Help?
                        break;
                    default: return;
                }
            }
        }
    }
}