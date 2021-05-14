using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Core.Data;
using AR.Bot.Domain;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Repositories
{
    // TODO: Refactor
    public interface ITelegramUserRepository : IRepository<TelegramUser>
    {
        Task<TelegramUser> GetOrCreate(long chatId);
    }

    public class TelegramUserRepositoryAsync : RepositoryAsync<TelegramUser>, ITelegramUserRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public TelegramUserRepositoryAsync(DataContext context, IUnitOfWork unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TelegramUser> GetOrCreate(long chatId)
        {
            var user = GetWithInclude(e => e.ChatId == chatId).FirstOrDefault();
            if (user != null) 
                return user;

            user = new TelegramUser(chatId);

            await AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return user;
        }
    }
}
