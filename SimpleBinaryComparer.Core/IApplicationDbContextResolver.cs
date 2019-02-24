namespace SimpleBinaryComparer.Core
{
    public interface IApplicationDbContextResolver
    {
        T GetCurrentDbContext<T>();
    }
}
