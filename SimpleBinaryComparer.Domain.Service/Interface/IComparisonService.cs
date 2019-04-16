using SimpleBinaryComparer.Domain.Service.Model;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.Domain.Service.Interface
{
    /// <summary>
    /// Comparison interface. Contains 2 method for saving and comparing arrays
    /// </summary>
    public interface IComparisonService
    {
        /// <summary>
        /// Saves comparison array to persistency
        /// </summary>
        /// <param name="comparison"></param>
        /// <returns></returns>
        Task InsertOrUpdateAsync(ComparisonInsertRequestDto comparison);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestDto"></param>
        /// <returns></returns>
        Task<ComparisonResponseDto> CompareAsync(ComparisonRequestDto requestDto);
    }
}
