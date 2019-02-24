using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleBinaryComparer.Core;
using SimpleBinaryComparer.Core.Repository;
using SimpleBinaryComparer.Domain.Repository.Context;
using SimpleBinaryComparer.Domain.Repository.Repository;
using SimpleBinaryComparer.Domain.Service;
using SimpleBinaryComparer.Domain.Service.Interface;

namespace SimpleBinaryComparer.Bootstrapper
{
    public class Registration
    {
        public void Register(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("ApplicationDbContext"));
            services.AddTransient<IComparisonService, ComparisonService>();
            services.AddTransient<IComparisonRepository, ComparisonRepository>();
            services.AddScoped<IApplicationDbContextResolver, ApplicationDbContextResolver>();

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

        }
    }
}
