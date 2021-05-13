using System;
using System.Threading.Tasks;
using AR.Bot.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AR.Bot.Web.Controllers
{
    [Route("api/bot")]
    public class BotController : Controller
    {
        private readonly ICallbackQueryHandler _callbackQueryHandler;
        private readonly IMessageHandler _messageHandler;

        public BotController(IMessageHandler messageHandler, ICallbackQueryHandler callbackQueryHandler)
        {
            _messageHandler       = messageHandler;
            _callbackQueryHandler = callbackQueryHandler;
        }

        [HttpGet]
        public IActionResult Root() => Ok();
        
        [HttpGet]
        [Route("ping")]
        public IActionResult Pong() => Ok("Pong");

        [HttpPost]
        public async Task<OkResult> Post([FromBody] Update update)
        {
            if (update == null) return Ok();

            switch (update.Type)
            {
                case UpdateType.Message:
                    await OnMessage(update.Message);
                    break;
                case UpdateType.CallbackQuery:
                    await OnCallbackQuery(update.CallbackQuery);
                    break;
            }

            return Ok();
        }

        private async Task OnCallbackQuery(CallbackQuery callbackQuery)
        {
            try
            {
                await _callbackQueryHandler.HandleCallbackQueryAsync(callbackQuery);
            }
            catch (UnsupportedCommand exception)
            {
                Log.Error(exception, "Got a CallbackQuery with unsupported command");
            }
            catch (UnsupportedMenuItem exception)
            {
                Log.Error(exception, "Got a CallbackQuery with unsupported item");
            }
            catch (MessageIsNotModifiedException exception)
            {
                Log.Error(exception, "Got a MessageIsNotModifiedException when tried to process CallbackQuery");
            }
        }

        private async Task OnMessage(Message message)
        {
            if (message.Date < Program.StartedTime - TimeSpan.FromSeconds(10))
                return;

            try
            {
                await _messageHandler.HandleMessageAsync(message);
            }
            catch (ApiRequestException exception) when (exception.Message == "Bad Request: reply message not found") { }
        }
    }
}
