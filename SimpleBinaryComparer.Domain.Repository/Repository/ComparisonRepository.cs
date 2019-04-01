using Microsoft.EntityFrameworkCore;
using SimpleBinaryComparer.Core;
using SimpleBinaryComparer.Core.Repository;
using SimpleBinaryComparer.Domain.Model;
using SimpleBinaryComparer.Domain.Repository.Context;
using System;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.Domain.Repository.Repository
{
    public class ComparisonRepository : IComparisonRepository
    {
        private readonly IApplicationDbContextResolver _applicationDbContextResolver;
        private ApplicationDbContext _dbContext;

        public ComparisonRepository(IApplicationDbContextResolver applicationDbContextResolver)
        {
            _applicationDbContextResolver = applicationDbContextResolver ?? throw new ArgumentNullException(nameof(applicationDbContextResolver));

            _dbContext = _applicationDbContextResolver.GetCurrentDbContext<ApplicationDbContext>();
        }

        public async Task<Comparison> GetByIdAsync(int id)
        {
            return await _dbContext.Comparisons.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task InsertAsync(Comparison comparison)
        {
            await _dbContext.AddAsync(comparison);
        }

        public void Update(Comparison comparison)
        {
            _dbContext.Update(comparison);
        }
    }
}
