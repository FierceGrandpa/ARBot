using AR.Bot.Core.Data;
using AR.Bot.Core.Menu;
using AR.Bot.Core.Services;
using AR.Bot.Domain;
using AR.Bot.Repositories;
using AR.Bot.Web.Validation;
using Flurl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

// ReSharper disable once CheckNamespace
namespace AR.Bot
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessageProcessors(this IServiceCollection services)
        {
            services.AddTransient<MessageValidator>();
            services.AddTransient<SettingsProcessor>();

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            services.AddTransient<IMessageHandler, MessageHandler>();
            services.AddTransient<ICallbackQueryHandler, CallbackQueryHandler>();

            return services;
        }

        // TODO: Make good with AOF
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IRepository<Activity>, RepositoryAsync<Activity>>();
            services.AddTransient<IRepository<Category>, RepositoryAsync<Category>>();
            services.AddTransient<IRepository<Skill>,    RepositoryAsync<Skill>>();
            
            services.AddTransient<ITelegramUserRepository, TelegramUserRepositoryAsync>();
            services.AddTransient<IDailyParcelRepository, DailyParcelRepositoryAsync>();

            services.AddTransient<IActivityService, ActivityService>();

            return services;
        }

        public static IServiceCollection AddMetrics(this IServiceCollection services)
        {
            // TODO: Add Metrics

            return services;
        }

        public static IServiceCollection AddTelegramBotClient(this IServiceCollection services, IConfiguration configuration)
        {
            TelegramOptions options = new();
            
            configuration.GetSection(nameof(TelegramOptions)).Bind(options);

            var client = new TelegramBotClient(options.BotToken);

            services.AddSingleton<ITelegramBotClient>(client);
            
            services.AddTransient<BotMenu>();
            
            var domain = options.WebhooksDomain;

            client.DeleteWebhookAsync().GetAwaiter().GetResult();
            client.SetWebhookAsync(domain.AppendPathSegments("api", "bot"), allowedUpdates: new[]
            {
                UpdateType.Message,
                UpdateType.CallbackQuery
            }).GetAwaiter().GetResult();



            return services;
        }
    }
}
