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

        /// <summary>
        /// returns current db context
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>T as current context</returns>
        public T GetCurrentDbContext<T>()
        {
            return (T)_dbContext;
        }
    }
}
