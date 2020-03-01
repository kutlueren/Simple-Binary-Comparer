using FluentAssertions;
using Moq;
using SimpleBinaryComparer.Core;
using SimpleBinaryComparer.Core.Repository;
using SimpleBinaryComparer.Domain.Model;
using SimpleBinaryComparer.Domain.Service;
using SimpleBinaryComparer.Domain.Service.Exception;
using SimpleBinaryComparer.Domain.Service.Model;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBinaryComparer.UnitTest
{
    public class ComparisonServiceTest
    {
        private Mock<IComparisonRepository> _comparisonRepository;
        protected Mock<IApplicationDbContext> _applicationDbContext;
        private Comparison comparison = null;
        private int id = 1;

        public ComparisonServiceTest()
        {
            _comparisonRepository = new Mock<IComparisonRepository>();
            _applicationDbContext = new Mock<IApplicationDbContext>();

            _comparisonRepository.Setup(t => t.InsertAsync(It.IsAny<Comparison>())).Callback<Comparison>((p) => { comparison = p; }).Returns(Task.CompletedTask);

            _comparisonRepository.Setup(t => t.GetByIdAsync(It.IsAny<int>())).Returns<int>(async (id) =>
            {
                if (comparison != null && comparison.Id == id)
                {
                    return await Task.FromResult<Comparison>(comparison);
                }

                return null;
            });

            _comparisonRepository.Setup(t => t.Update(It.IsAny<Comparison>())).Callback<Comparison>((p) => { comparison = p; });
        }

        [Fact]
        public void Ctor_Test()
        {
            Assert.Throws<ArgumentNullException>(() => { new ComparisonService(null, null); });

            Assert.Throws<ArgumentNullException>(() => { new ComparisonService(_comparisonRepository.Object, null); });
        }

        [Theory]
        [InlineData(new byte[3] { 1, 2, 3 }, ComparisonEnum.Left)]
        [InlineData(new byte[3] { 1, 2, 3 }, ComparisonEnum.Right)]
        public async Task ComparisonService_Should_Insert_ComparisonAsync(byte[] value, ComparisonEnum type)
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = value, ValueType = type });

            comparison.Should().NotBeNull();

            if (type == ComparisonEnum.Left)
            {
                comparison.LeftArray.Should().Equal(value);
            }
            else
            {
                comparison.RightArray.Should().Equal(value);
            }
        }

        [Theory]
        [InlineData(new byte[3] { 1, 2, 3 }, ComparisonEnum.Left)]
        [InlineData(new byte[3] { 1, 2, 3 }, ComparisonEnum.Right)]
        public async Task ComparisonService_Should_Update_ComparisonAsync(byte[] value, ComparisonEnum type)
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = value, ValueType = type });

            byte[] newValue = new byte[4] { 1, 2, 3, 4 };

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = newValue, ValueType = type });

            comparison.Should().NotBeNull();

            if (type == ComparisonEnum.Left)
            {
                comparison.LeftArray.Should().Equal(newValue);
            }
            else
            {
                comparison.RightArray.Should().Equal(newValue);
            }
        }

        [Fact]
        public async Task ComparisonService_Comparison_ShouldThrow_Exception_Due_To_Null_Comparison_Value()
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            BusinessException ex = await Assert.ThrowsAsync<BusinessException>(async () => { await comparisonService.CompareAsync(new ComparisonRequestDto() { Id = 1, ValueType = ComparisonEnum.Left }); });

            ex.Message.Should().Equals("No record found!");
        }

        [Fact]
        public async Task ComparisonService_Right_Comparison_Should_Throw_Exception_Due_To_Null_Right_Value()
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            byte[] value = new byte[4] { 1, 2, 3, 4 };

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = value, ValueType = ComparisonEnum.Left });

            BusinessException ex = await Assert.ThrowsAsync<BusinessException>(async () => { await comparisonService.CompareAsync(new ComparisonRequestDto() { Id = 1, ValueType = ComparisonEnum.Left }); });

            ex.Message.Should().Equals("Right array is null!");
        }

        [Fact]
        public async Task ComparisonService_Left_Comparison_Should_Throw_Exception_Due_To_Null_Left_Value()
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            byte[] value = new byte[4] { 1, 2, 3, 4 };

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = value, ValueType = ComparisonEnum.Right });

            BusinessException ex = await Assert.ThrowsAsync<BusinessException>(async () => { await comparisonService.CompareAsync(new ComparisonRequestDto() { Id = 1, ValueType = ComparisonEnum.Right }); });

            ex.Message.Should().Equals("Left array is null!");
        }

        [Theory]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[4] { 1, 2, 3, 4 }, ComparisonEnum.Left)]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[4] { 1, 2, 3, 4 }, ComparisonEnum.Right)]
        public async Task ComparisonService_ComparisonAsync_Should_Return_SameSize_False(byte[] valueLeft, byte[] valueRight, ComparisonEnum type)
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = valueLeft, ValueType = ComparisonEnum.Left });

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = valueRight, ValueType = ComparisonEnum.Right });

            comparison.Should().NotBeNull();

            ComparisonResponseDto result = await comparisonService.CompareAsync(new ComparisonRequestDto() { Id = id, ValueType = type });
            ComparisonResponseObject resultObject = (ComparisonResponseObject)result.Result;

            resultObject.Equal.Should().BeFalse();
            resultObject.SameSize.Should().BeFalse();
        }

        [Theory]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[3] { 1, 2, 3 }, ComparisonEnum.Left)]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[3] { 1, 2, 3 }, ComparisonEnum.Right)]
        public async Task ComparisonService_ComparisonAsync_Should_Return_Equal_True(byte[] valueLeft, byte[] valueRight, ComparisonEnum type)
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = valueLeft, ValueType = ComparisonEnum.Left });

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = valueRight, ValueType = ComparisonEnum.Right });

            comparison.Should().NotBeNull();

            ComparisonResponseDto result = await comparisonService.CompareAsync(new ComparisonRequestDto() { Id = id, ValueType = type });
            ComparisonResponseObject resultObject = (ComparisonResponseObject)result.Result;

            resultObject.Equal.Should().BeFalse();
            resultObject.SameSize.Should().BeTrue();
        }

        [Theory]
        [InlineData(new byte[4] { 7, 1, 2, 5 }, new byte[4] { 1, 2, 4, 8 }, ComparisonEnum.Left)]
        [InlineData(new byte[4] { 7, 1, 2, 5 }, new byte[4] { 1, 2, 4, 8 }, ComparisonEnum.Right)]
        public async Task ComparisonService_ComparisonAsync_Should_Return_Equal_False(byte[] valueLeft, byte[] valueRight, ComparisonEnum type)
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = valueLeft, ValueType = ComparisonEnum.Left });

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = valueRight, ValueType = ComparisonEnum.Right });

            comparison.Should().NotBeNull();

            ComparisonResponseDto result = await comparisonService.CompareAsync(new ComparisonRequestDto() { Id = id, ValueType = type });
            ComparisonResponseObject resultObject = (ComparisonResponseObject)result.Result;

            resultObject.Equal.Should().BeFalse();
            resultObject.SameSize.Should().BeTrue();
            resultObject.Difference.Length.Should().Be(2);
            var diffList = resultObject.Difference.OffSets.ToList();

            if (type == ComparisonEnum.Left)
            {
                diffList[0].OffSet.Should().Be(0);
                diffList[1].OffSet.Should().Be(3);
            }
            else
            {
                diffList[0].OffSet.Should().Be(2);
                diffList[1].OffSet.Should().Be(3);
            }
        }
    }
}