using SimpleBinaryComparer.Domain.Model;
using SimpleBinaryComparer.Domain.Service.Model;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.Domain.Service.Interface
{
    public interface IComparisonService
    {
        Task InsertOrUpdateAsync(ComparisonInsertRequestDto comparison);
        Task<ComparisonResponseDto> CompareAsync(ComparisonRequestDto requestDto);
    }
}
