using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UserServiceLibrary.Configuration;
using UserServiceLibrary.Models;
using UserServiceLibrary.Services;

namespace UserServiceTests
{
    public class ExternalUserServiceTests
    {
        /// <summary>
        /// Creates an instance of ExternalUserService with a mocked HTTP response.
        /// </summary>
        /// <param name="jsonResponse"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        private ExternalUserService CreateServiceWithMockedResponse(string jsonResponse, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(jsonResponse),
                });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://reqres.in/api/")
            };

            var options = Options.Create(new ApiSettings
            {
                BaseUrl = "https://reqres.in/api/"
            });
            return new ExternalUserService(httpClient, options);
        }

        /// <summary>
        /// Tests that GetUserByIdAsync returns a user when a valid ID is provided.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetUserByIdAsync_ReturnsUser_WhenValidId()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                First_Name = "George",
                Last_Name = "Bluth",
                Email = "george.bluth@reqres.in",
                Avatar = "https://reqres.in/img/faces/1-image.jpg"
            };

            var json = JsonSerializer.Serialize(new Dictionary<string, User> { { "data", user } });
            var service = CreateServiceWithMockedResponse(json);

            // Act
            var result = await service.GetUserByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("George", result.First_Name);
        }

        /// <summary>
        /// Tests that GetUserByIdAsync throws an exception when an invalid ID is provided.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllUsersAsync_ReturnsAllUsersAcrossPages()
        {
            // Arrange
            var response1 = new UserListResponse
            {
                Page = 1,
                Total_Pages = 2,
                Data = new List<User> { new User { Id = 1, First_Name = "John" } }
            };

            var response2 = new UserListResponse
            {
                Page = 2,
                Total_Pages = 2,
                Data = new List<User> { new User { Id = 2, First_Name = "Jane" } }
            };

            var queue = new Queue<HttpResponseMessage>(new[]
            {
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(response1)) },
                new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(JsonSerializer.Serialize(response2)) },
            });

            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(() => queue.Dequeue());

            var client = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://reqres.in/api/")
            };

            var options = Options.Create(new ApiSettings
            {
                BaseUrl = "https://reqres.in/api/"
            });
            var service = new ExternalUserService(client, options);

            // Act
            var users = await service.GetAllUsersAsync();

            // Assert
            Assert.NotNull(users);
            Assert.Collection(users,
                u => Assert.Equal(1, u.Id),
                u => Assert.Equal(2, u.Id));
        }
    }
}
