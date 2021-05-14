using System;
using System.Threading.Tasks;
using AR.Bot.Domain;
using AR.Bot.Repositories;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Services
{
    public enum Setting
    {
        MailingMode
    }
    
    public class SettingsProcessor
    {
        // TODO: Inject
        private readonly ITelegramUserRepository _userRepository;
        private readonly IUnitOfWork             _unitOfWork;
        public SettingsProcessor(ITelegramUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork     = unitOfWork;
        }
        
        public async Task<SendingTimeMode> GetMailingMode(long chatId)
        {
            var user = await _userRepository.GetOrCreate(chatId);
            return user.MailingMode;
        }

        public async Task ChangeMailingMode(long chatId, SendingTimeMode mode)
        {
            var user = await _userRepository.GetOrCreate(chatId);
            if (user.MailingMode == mode)
                return;

            user.MailingMode = mode;

            await _userRepository.UpdateAsync(user.Id, user);
            await _unitOfWork.SaveChangesAsync();
        }

        public bool ValidateSettings(Setting setting, string value) =>
            setting switch
            {
                Setting.MailingMode => Enum.TryParse(typeof(SendingTimeMode), value, true, out _),
                _ => false
            };
    }
}