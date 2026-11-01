using Microsoft.AspNetCore.Mvc.Testing;
using ServiceRequestTracker.API.Requests;
using ServiceRequestTracker.Common.Enums;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace ServiceRequestTracker.API.IntegrationTests
{
    public class ServiceRequestTrackerApiTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ServiceRequestTrackerApiTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ServiceRequests_Returns_Success()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("api/service-requests?Page=1&PageSize=10");
            // Verify
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_ServiceRequest_by_Id_Returns_NotFound()
        {
            // Arrange
            var serviceRequestId = Guid.NewGuid();
            // Act
            var response = await _client.GetAsync($"api/service-requests/{serviceRequestId}");
            // Verify
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_ServiceRequests_Returns_Success()
        {
            // Arrange
            var request = new CreateServiceRequest
            {
                Title = "Test Service Request",
                Description = "This is a test service request description",
                Priority = Priority.Normal
            };

            // Act
            var response = await _client.PostAsJsonAsync("api/service-requests", request);
            // Verify
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Update_ServiceRequests_Returns_NotFound()
        {
            // Arrange
            var serviceRequestId = Guid.NewGuid();
            var emptyContent = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            // Act
            var response = await _client.PatchAsync($"api/service-requests/{serviceRequestId}/status?status={Status.InProgress}", emptyContent);
            // Verify
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
