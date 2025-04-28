using Moq;
using System.Net;
using ExternalUserIntegration.Clients;
using ExternalUserIntegration.Configuration;
using Moq.Protected;
using Microsoft.Extensions.Options;

public class ReqresApiClientTests
{
	[Fact]
	public async Task GetAllUsersAsync_ShouldReturnUsers()
	{
		// Arrange
		var mockHttpClientHandler = new Mock<HttpMessageHandler>();

		// First page mock response
		var firstPageResponse = new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.OK,
			Content = new StringContent("{ \"page\": 1, \"total_pages\": 2, \"data\": [{ \"id\": 1, \"first_name\": \"John\", \"last_name\": \"Doe\", \"email\": \"john.doe@reqres.in\" }] }", System.Text.Encoding.UTF8, "application/json")
		};

		// Second page mock response
		var secondPageResponse = new HttpResponseMessage
		{
			StatusCode = HttpStatusCode.OK,
			Content = new StringContent("{ \"page\": 2, \"total_pages\": 2, \"data\": [{ \"id\": 2, \"first_name\": \"Jane\", \"last_name\": \"Smith\", \"email\": \"jane.smith@reqres.in\" }] }", System.Text.Encoding.UTF8, "application/json")
		};

		// Mock the behavior of SendAsync to return different responses based on the page
		mockHttpClientHandler
			.Protected()
			.Setup<Task<HttpResponseMessage>>(
				"SendAsync",
				ItExpr.IsAny<HttpRequestMessage>(),
				ItExpr.IsAny<CancellationToken>())
			.ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
			{
				if (request.RequestUri.ToString().Contains("page=1"))
					return firstPageResponse;
				else if (request.RequestUri.ToString().Contains("page=2"))
					return secondPageResponse;
				else
					return new HttpResponseMessage(HttpStatusCode.BadRequest);
			});

		// Mock the IOptions for ReqresApiOptions
		var mockOptions = new Mock<IOptions<ReqresApiOptions>>();
		mockOptions.Setup(o => o.Value).Returns(new ReqresApiOptions { BaseUrl = "https://reqres.in/api" });

		// Create the HttpClient and ReqresApiClient
		var httpClient = new HttpClient(mockHttpClientHandler.Object);
		var client = new ReqresApiClient(httpClient, mockOptions.Object);

		// Act
		var usersResponse = await client.GetAllUsersAsync();

		// Assert
		Assert.NotNull(usersResponse);
		Assert.Equal(2, usersResponse.Count); // Two users are returned across two pages
		Assert.Equal("John", usersResponse[0].First_Name);
		Assert.Equal("Jane", usersResponse[1].First_Name);
	}
}
