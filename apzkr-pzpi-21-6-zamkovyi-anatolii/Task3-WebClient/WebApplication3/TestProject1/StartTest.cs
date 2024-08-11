using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Controllers;
using WebApplication3.Data;
using WebApplication3.Models;

namespace TestProject1
{
    public class StartTest
    {
        private readonly CrewHandicapsController _controller;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly Mock<ApiSettings> _mockApiSettings;

        public StartTest()
        {
            var options = WebApplication3Context.GetSqlServerOptions();
            var context = new WebApplication3Context(options);

            _mockHttpClientFactory = new Mock<IHttpClientFactory>();

            _mockApiSettings = new Mock<IOptions<ApiSettings>>();
            _mockApiSettings.Setup(a => a.Value).Returns(new ApiSettings
            {
                BaseApiUrl = "https://localhost:7237/api"
            });

            _controller = new CrewHandicapsController(context, _mockHttpClientFactory.Object, _mockApiSettings.Object);
        }
        [Fact]
        public async Task AssignRandomStartNumbers_ShouldRedirectToIndex_OnSuccess()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(client => client.PostAsync(It.IsAny<string>(), null)).ReturnsAsync(mockResponse);
            _mockHttpClientFactory.Setup(factory => factory.CreateClient()).Returns(mockClient.Object);

            // Act
            var result = await _controller.AssignRandomStartNumbers(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task AssignRandomStartNumbers_ShouldReturnError_OnFailure()
        {
            // Arrange
            var mockResponse = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Error")
            };
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(client => client.PostAsync(It.IsAny<string>(), null)).ReturnsAsync(mockResponse);
            _mockHttpClientFactory.Setup(factory => factory.CreateClient()).Returns(mockClient.Object);

            // Act
            var result = await _controller.AssignRandomStartNumbers(1) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Error assigning start numbers: Error", result.Value);
        }

        [Fact]
        public async Task AssignRandomStartNumbers_ShouldReturnServerError_OnException()
        {
            // Arrange
            var mockClient = new Mock<HttpClient>();
            mockClient.Setup(client => client.PostAsync(It.IsAny<string>(), null)).ThrowsAsync(new System.Exception("Exception"));
            _mockHttpClientFactory.Setup(factory => factory.CreateClient()).Returns(mockClient.Object);

            // Act
            var result = await _controller.AssignRandomStartNumbers(1) as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(500, result.StatusCode);
            Assert.Equal("Internal server error: Exception", result.Value);
        }
    }
}
