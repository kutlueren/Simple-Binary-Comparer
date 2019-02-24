using System.Threading.Tasks;

namespace SimpleBinaryComparer.Core
{
    public interface IApplicationDbContext
    {
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
