# Simple Binary Comparer

This project contains solutions to the "Assignment Scalable Web". It consists of mainly 3 projects, one for backend API (SimpleBinaryComparer.API), one for integration test, also an unit test project for backend API.

## Getting Started

This solutions are implemented in order to test and achieve the requirements described in the assignment. All the requirements are covered with the solution which includes a REST API and documentation with Swagger, 
unit and intergration testing with xunit, a simple layered approach for the arthitecture and repository pattern for persistance and a in memory database.

## Tech/framework used
The techs used in the solution as follows;

	1. .Net core 2.2
	2. .Net standart 2.0
	3. xunit
	4. Moq
	5. FluentAssertions
	6. Git
	7. Visual Studio Community 2017
	8. Swagger for API
	9. Entity Framework core and in memory database as context


## How is it done?

### REST API

So the REST API is SimpleBinaryComparer.API and it is implemented using .net core. Tha design of the system lays essentially like following;

 **a. SimpleBinaryComparer.Bootstrapper :** Used for dependency injection implementation. Consequently, rest of the system is abstracted from the API except SimpleBinaryComparer.Domain.Service.

 Microsoft.Extensions.DependencyInjection library used for dependency injection and it has been implemented such as;

 		 services.AddDbContext<ApplicationDbContext>(opt => opt.UseInMemoryDatabase("ApplicationDbContext"));
         services.AddTransient<IComparisonService, ComparisonService>();

 **b. SimpleBinaryComparer.Domain.Service:** Used for service layer, aka business layer, orchestrates the repositories and talks to the persistance layer via them. Transforms domain model entities to UI dto's and vice versa.

 **c. SimpleBinaryComparer.Domain.Repository:** Used to implement persistance layer, an in-memory database is used to store the data.

 Registered objects search has been implemented as follows to meet with the description in the assignment;

 	    public async Task<Comparison> GetByIdAsync(int id)
        {
            return await _dbContext.Comparisons.FirstOrDefaultAsync(t => t.Id == id);
        }

 **d. SimpleBinaryComparer.Domain.Model:** Domain model for the system. 

 **e. SimpleBinaryComparer.Core:** A seperated layer containing interfaces to implement repositories and db context. Consequently, it enables us to change the repository layer in the future if needed, further implementations might be needed for that as well. There is no direct dependency between SimpleBinaryComparer.Domain.Service and SimpleBinaryComparer.Domain.Repository.

 **f. SimpleBinaryComparer.UnitTest:** A xunit test project to test the services and controller. There are 10 tests implemented but with inline functionality, it expands to 15. 
 IComparisonRepository and IApplicationDbContext are mocked usign Moq and served as fake data generators. There is only one test for controller since the API is under integration test.
 The following test is a sample of assertioning of not equal size of 2 arrays;

 		[Theory]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[4] { 1, 2, 3, 4 }, ComparisonEnum.Left)]
        [InlineData(new byte[3] { 1, 2, 3 }, new byte[4] { 1, 2, 3, 4 }, ComparisonEnum.Right)]
        public async Task ComparisonService_ComparisonAsync_Should_Return_SameSize_False(byte[] valueLeft, byte[] valueRight, ComparisonEnum type)
        {
            ComparisonService comparisonService = new ComparisonService(_comparisonRepository.Object, _applicationDbContext.Object);

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = valueLeft, ValueType = ComparisonEnum.Left });

            await comparisonService.InsertOrUpdateAsync(new ComparisonInsertRequestDto() { Id = id, Value = valueRight, ValueType = ComparisonEnum.Right });

            comparison.Should().NotBeNull();

            ComparisonResponseDto result= await comparisonService.CompareAsync(new ComparisonRequestDto() { Id = id, ValueType = type });
            ComparisonResponseObject resultObject = (ComparisonResponseObject)result.Result;

            resultObject.Equal.Should().BeFalse();
            resultObject.SameSize.Should().BeFalse(); 
        }
		
 **g. SimpleBinaryComparer.IntegrationTest:** A xunit test project to test the API. There are 7 tests implemented but with inline functionality, it expands to 11. 
 The following test is a sample of assertioning of not equalality of 2 arrays;
 
 
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
 
 **h. SimpleBinaryComparer.API:** it is the REST API implemented in .net core. There are basically 3 methods than a client can consume. You can find the methods in DiffController. 

 **NOTE!!: Since the assignment is open for assumption, I have implemented the api, business logic, reqeust and respone objects according to my assumption. Left method saves the array as left array, Right method does the same
 for right array. Only one object gets created and saved to persistance for left and rigth array per id. If there is already created object, the left and right methods updates the array aocordingly. Get method only compares the
 arrays if the 2 arrays exists in the object. If not, then an exception gets thrown. If there is no ojects in given id then an exception thrown as well.**
 
 1. Get method is used to make comparison between 2 arrays. You have to indicate the comparison type in the request object whether it is going to find diffs in the left array or right array. "1" should be send in order to
 find the diffs in the left array, "2" for the right array. It only returns the offsetts of the diffs and the length of the diff data.
 2. Left method saves or updates the array to left.
 3. Right method saves or updates the array to right.
 
 There is a middleware for global exception handling as well(ExceptionMiddleware). It handles the exceptions and returns a proper response the the client.

 The api will run on http://localhost:53040 and if you type http://localhost:53040/swagger/index.html you can observe the api methods which are;
 
		1. /v1/diff/{id}
		2. /v1/diff/{id}/left
		3. /v1/diff/{id}/right

## Credits

	Aram Koukia 	https://koukia.ca/integration-testing-in-asp-net-core-2-0-51d14ede3968
					https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2
					https://github.com/aspnet/Docs/blob/master/aspnetcore/test/integration-tests.md
