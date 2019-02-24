using SimpleBinaryComparer.Core;
using SimpleBinaryComparer.Core.Repository;
using SimpleBinaryComparer.Domain.Model;
using SimpleBinaryComparer.Domain.Service.Exception;
using SimpleBinaryComparer.Domain.Service.Interface;
using SimpleBinaryComparer.Domain.Service.Model;
using System;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.Domain.Service
{
    public class ComparisonService : IComparisonService
    {
        private readonly IComparisonRepository _comparisonRepository;
        private readonly IApplicationDbContext _applicationDbContext;

        public ComparisonService(IComparisonRepository comparisonRepository, IApplicationDbContext applicationDbContext)
        {
            _comparisonRepository = comparisonRepository ?? throw new ArgumentNullException(nameof(comparisonRepository));
            _applicationDbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
        }

        public async Task<ComparisonResponseDto> CompareAsync(ComparisonRequestDto requestDto)
        {
            Comparison comparison = await _comparisonRepository.GetByIdAsync(requestDto.Id);

            if (comparison == null)
            {
                throw new BusinessException("No record found!");
            }

            if (comparison.LeftArray == null)
            {
                throw new BusinessException("Left array is null!");
            }

            if (comparison.RightArray == null)
            {
                throw new BusinessException("Right array is null!");
            }

            ComparisonResponseDto responseDto = new ComparisonResponseDto();
            ComparisonResponseObject responseObject = new ComparisonResponseObject();

            responseDto.Success = true;

            // If not of equal size just return that
            if (!comparison.EqualSize())
            {
                responseObject.Equal = false;
                responseObject.SameSize = false;

                responseDto.Result = responseObject;

                return responseDto;
            }

            responseObject.SameSize = true;

            // If equal return that
            if (!comparison.IsEqual())
            {
                responseObject.Equal = false;
            }

            // If of same size provide insight in where the diffs are, actual diffs are not needed.
            if (requestDto.ValueType == ComparisonEnum.Left)
            {
                responseObject.Difference = comparison.FindDiffsInLeft();

                responseDto.Result = responseObject;

                return responseDto;
            }
            else
            {
                responseObject.Difference = comparison.FindDiffsInRight();

                responseDto.Result = responseObject;

                return responseDto;
            }
        }

        public async Task InsertOrUpdateAsync(ComparisonInsertRequestDto requestDto)
        {
            Comparison comparison = await _comparisonRepository.GetByIdAsync(requestDto.Id);
            bool insert = false;

            if (comparison == null)
            {
                comparison = new Comparison();
                comparison.Id = requestDto.Id;
                insert = true;
            }

            if (requestDto.ValueType == ComparisonEnum.Left)
            {
                comparison.LeftArray = requestDto.Value;
            }
            else
            {
                comparison.RightArray = requestDto.Value;
            }

            if (insert)
            {
                await _comparisonRepository.InsertAsync(comparison);
            }
            else
            {
                _comparisonRepository.Update(comparison);
            }

            await _applicationDbContext.SaveChangesAsync();
        }
    }
}
