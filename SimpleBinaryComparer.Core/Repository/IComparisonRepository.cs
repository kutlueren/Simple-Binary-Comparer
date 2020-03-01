using SimpleBinaryComparer.Domain.Model;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.Core.Repository
{
    /// <summary>
    /// Comparison Repository abstraction
    /// </summary>
    public interface IComparisonRepository
    {
        /// <summary>
        /// Gets array by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Comparison</returns>
        Task<Comparison> GetByIdAsync(int id);

        /// <summary>
        /// Inserts array to db
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        Task InsertAsync(Comparison comparison);

        /// <summary>
        /// Updates existing array
        /// </summary>
        /// <param name="comparison"></param>
        void Update(Comparison comparison);
    }
}