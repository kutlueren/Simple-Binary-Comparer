using Microsoft.EntityFrameworkCore;
using SimpleBinaryComparer.Core;
using SimpleBinaryComparer.Domain.Model;
using System.Reflection;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.Domain.Repository.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<Comparison> Comparisons { get; set; }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await base.SaveChangesAsync();
        }
    }
}
