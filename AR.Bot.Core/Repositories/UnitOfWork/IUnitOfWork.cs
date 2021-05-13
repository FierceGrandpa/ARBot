using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}