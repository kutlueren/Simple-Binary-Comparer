using Microsoft.AspNetCore.Mvc;
using SimpleBinaryComparer.API.Model;
using SimpleBinaryComparer.Domain.Service.Interface;
using SimpleBinaryComparer.Domain.Service.Model;
using System;
using System.Threading.Tasks;

namespace SimpleBinaryComparer.API.Controllers
{
    [Route("v1/diff/")]
    [ApiController]
    public class DiffController : ControllerBase
    {
        private readonly IComparisonService _comparisonService;

        public DiffController(IComparisonService comparisonService)
        {
            _comparisonService = comparisonService ?? throw new ArgumentNullException(nameof(comparisonService));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Get(int id, [FromBody] CompareRequestModel value)
        {
            ComparisonResponseDto response = await _comparisonService.CompareAsync(new ComparisonRequestDto() { Id = id, ValueType = value.Type });

            return Ok(response);
        }

        [HttpPost("{id}/left")]
        public async Task<IActionResult> Left(int id, [FromBody] RequestModel value)
        {
            ComparisonInsertRequestDto requestDto = new ComparisonInsertRequestDto();
            requestDto.Value = value.Value;
            requestDto.ValueType = ComparisonEnum.Left;
            requestDto.Id = id;

            await _comparisonService.InsertOrUpdateAsync(requestDto);

            return Ok(new ResponseBase() { Success = true });
        }

        [HttpPost("{id}/right")]
        public async Task<IActionResult> Right(int id, [FromBody] RequestModel value)
        {
            ComparisonInsertRequestDto requestDto = new ComparisonInsertRequestDto();
            requestDto.Value = value.Value;
            requestDto.ValueType = ComparisonEnum.Right;
            requestDto.Id = id;

            await _comparisonService.InsertOrUpdateAsync(requestDto);

            return Ok(new ResponseBase() { Success = true });
        }
    }
}
