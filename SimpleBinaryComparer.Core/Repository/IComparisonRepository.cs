using SimpleBinaryComparer.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.Core.Repository
{
    public interface IComparisonRepository
    {
        Task<Comparison> GetByIdAsync(int id);

        Task InsertAsync(Comparison comparison);

        void Update(Comparison comparison);
    }
}
