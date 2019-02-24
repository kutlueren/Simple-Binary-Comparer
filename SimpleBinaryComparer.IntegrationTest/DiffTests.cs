using FluentAssertions;
using Newtonsoft.Json;
using SimpleBinaryComparer.Domain.Service.Model;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleBinaryComparer.IntegrationTest
{
    public class DiffTests
    {
        private HttpClient httpClient = new HttpClient();
        private string host = "http://localhost:53040/v1/diff";

        public DiffTests()
        {
            httpClient.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        [Theory]
        [InlineData(1, new byte[4] { 1, 2, 3, 4 })]
        public async Task Should_Save_Left(int id, byte[] value)
        {
            RequestModel requestModel = new RequestModel();

            requestModel.Value = value;

            HttpResponseMessage response = await httpClient.PostAsync($"{host}/{id}/left", new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData(1, new byte[4] { 1, 2, 3, 4 })]
        public async Task Should_Save_Right(int id, byte[] value)
        {
            RequestModel requestModel = new RequestModel();

            requestModel.Value = value;

            HttpResponseMessage response = await httpClient.PostAsync($"{host}/{id}/right", new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponseBase servicResponse = await response.ToResponseBaseAsync();

            servicResponse.Success.Should().BeTrue();
        }

        [Fact]
        public async Task Compare_Should_Return_500_No_Record_Found()
        {
            CompareRequestModel requestModel = new CompareRequestModel();

            requestModel.Type = ComparisonEnum.Left;

            HttpResponseMessage response = await httpClient.PostAsync($"{host}/2", new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            ResponseBase servicResponse = await response.ToResponseBaseAsync();

            servicResponse.Success.Should().BeFalse();

            servicResponse.Message.Should().Be("No record found!");
        }

        [Theory]
        [InlineData(ComparisonEnum.Left, 3)]
        [InlineData(ComparisonEnum.Right, 4)]
        public async Task Comparison_Should_Throw_Exception_Due_To_Null_Right_Or_Left_Value(ComparisonEnum value, int id)
        {
            if (value == ComparisonEnum.Left)
            {
                await Should_Save_Left(id, new byte[4] { 1, 2, 3, 4 });
            }
            else
            {
                await Should_Save_Right(id, new byte[4] { 1, 2, 3, 4 });
            }

            CompareRequestModel requestModel = new CompareRequestModel();

            requestModel.Type = ComparisonEnum.Left;

            HttpResponseMessage response = await httpClient.PostAsync($"{host}/{id}", new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

            ResponseBase servicResponse = await response.ToResponseBaseAsync();

            servicResponse.Success.Should().BeFalse();

            if (value == ComparisonEnum.Left)
            {
                servicResponse.Message.Should().Be("Right array is null!");
            }
            else
            {
                servicResponse.Message.Should().Be("Left array is null!");

            }
        }


        [Theory]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[4] { 1, 2, 3, 4 }, ComparisonEnum.Left, 8)]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[4] { 1, 2, 3, 4 }, ComparisonEnum.Right, 8)]
        public async Task Comparison_Should_Return_SameSize_False(byte[] valueLeft, byte[] valueRight, ComparisonEnum type, int id)
        {
            await Should_Save_Left(id, valueLeft);

            await Should_Save_Right(id, valueRight);

            CompareRequestModel requestModel = new CompareRequestModel();

            requestModel.Type = type;

            HttpResponseMessage response = await httpClient.PostAsync($"{host}/{id}", new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponseBase servicResponse = await response.ToResponseBaseAsync();

            servicResponse.Success.Should().BeTrue();

            ComparisonResponseObject resultObject = JsonConvert.DeserializeObject<ComparisonResponseObject>(servicResponse.Result.ToString());

            resultObject.Equal.Should().BeFalse();
            resultObject.SameSize.Should().BeFalse();
        }

        [Theory]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[3] { 1, 2, 3 }, ComparisonEnum.Left, 9)]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[3] { 1, 2, 3 }, ComparisonEnum.Right, 9)]
        public async Task Comparison_Should_Return_Equal_True(byte[] valueLeft, byte[] valueRight, ComparisonEnum type, int id)
        {
            await Should_Save_Left(id, valueLeft);

            await Should_Save_Right(id, valueRight);

            CompareRequestModel requestModel = new CompareRequestModel();

            requestModel.Type = type;

            HttpResponseMessage response = await httpClient.PostAsync($"{host}/{id}", new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponseBase servicResponse = await response.ToResponseBaseAsync();

            servicResponse.Success.Should().BeTrue();

            ComparisonResponseObject resultObject = JsonConvert.DeserializeObject<ComparisonResponseObject>(servicResponse.Result.ToString());

            resultObject.Equal.Should().BeFalse();
            resultObject.SameSize.Should().BeTrue();
        }

        [Theory]
        [InlineData(new byte[4] { 7, 1, 2, 5 }, new byte[4] { 1, 2, 4, 8 }, ComparisonEnum.Left, 10)]
        [InlineData(new byte[4] { 7, 1, 2, 5 }, new byte[4] { 1, 2, 4, 8 }, ComparisonEnum.Right, 10)]
        public async Task Comparison_Should_Return_Equal_False(byte[] valueLeft, byte[] valueRight, ComparisonEnum type, int id)
        {

            await Should_Save_Left(id, valueLeft);

            await Should_Save_Right(id, valueRight);

            CompareRequestModel requestModel = new CompareRequestModel();

            requestModel.Type = type;

            HttpResponseMessage response = await httpClient.PostAsync($"{host}/{id}", new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json"));

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            ResponseBase servicResponse = await response.ToResponseBaseAsync();

            servicResponse.Success.Should().BeTrue();

            ComparisonResponseObject resultObject = JsonConvert.DeserializeObject<ComparisonResponseObject>(servicResponse.Result.ToString());

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


    public class RequestModel
    {
        public byte[] Value { get; set; }
    }

    public class CompareRequestModel
    {
        public ComparisonEnum Type { get; set; }
    }
}
