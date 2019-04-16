namespace SimpleBinaryComparer.Core
{
    /// <summary>
    /// An abstraction to resolve db context for repositories
    /// </summary>
    public interface IApplicationDbContextResolver
    {
        T GetCurrentDbContext<T>();
    }
}
