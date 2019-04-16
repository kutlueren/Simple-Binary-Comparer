using SimpleBinaryComparer.Core;

namespace SimpleBinaryComparer.Domain.Repository.Context
{
    public class ApplicationDbContextResolver : IApplicationDbContextResolver
    {
        private readonly IApplicationDbContext _dbContext;

        public ApplicationDbContextResolver(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetCurrentDbContext<T>()
        {
            return (T)_dbContext;
        }
    }
}
