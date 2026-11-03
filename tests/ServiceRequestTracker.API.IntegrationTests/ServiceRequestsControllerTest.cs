using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ServiceRequestTracker.API.Controllers;
using ServiceRequestTracker.API.Responses;
using ServiceRequestTracker.API.Services;
using ServiceRequestTracker.Common.Enums;

namespace ServiceRequestTracker.API.IntegrationTests
{
    public class ServiceRequestsControllerTest
    {
        private readonly ServiceRequestsController _controller;
        private readonly Mock<IServiceRequestsService> _serviceMock;
        private readonly Fixture _fixture = new();

        public ServiceRequestsControllerTest()
        {
            _serviceMock = new Mock<IServiceRequestsService>();
            _controller = new ServiceRequestsController(_serviceMock.Object);
        }

        [Fact]
        public async Task Get_ServiceRequest_by_Id_Returns_Ok()
        {
            // Arrange
            var serviceRequestId = Guid.NewGuid();
            var expectedServiceRequest = _fixture.Build<GetServiceRequestResponse>()
                .With(sr => sr.Id, serviceRequestId).Create();
            _serviceMock.Setup(sm => sm.GetServiceRequest(serviceRequestId)).ReturnsAsync(expectedServiceRequest);
            // Act
            var response = await _controller.Get(serviceRequestId);
            var actualServiceRequestResponse = response.Result.As<OkObjectResult>().Value.As<GetServiceRequestResponse>();
            // Verify
            actualServiceRequestResponse.Should().BeEquivalentTo(expectedServiceRequest);
        }

        [Theory]
        [InlineData(Status.Cancelled, Status.InProgress)]
        [InlineData(Status.Cancelled, Status.Done)]
        [InlineData(Status.Done, Status.New)]
        [InlineData(Status.New, Status.Done)]
        [InlineData(Status.InProgress, Status.New)]
        [InlineData(Status.Done, Status.Cancelled)]
        public async Task Update_ServiceRequests_Returns_Conflict(Status currentStatus, Status newStatus)
        {
            // Arrange
            var serviceRequestId = Guid.NewGuid();
            var expectedServiceRequest = _fixture.Build<GetServiceRequestResponse>()
                .With(sr => sr.Id, serviceRequestId).With(sr => sr.Status, currentStatus).Create();
            _serviceMock.Setup(sm => sm.GetServiceRequest(serviceRequestId)).ReturnsAsync(expectedServiceRequest);
            // Act
            var response = await _controller.Update(serviceRequestId, newStatus);
            // Verify
            Assert.IsType<ConflictObjectResult>(response);
        }

        [Theory]
        [InlineData(Status.New, Status.InProgress)]
        [InlineData(Status.New, Status.Cancelled)]
        [InlineData(Status.InProgress, Status.Done)]
        [InlineData(Status.InProgress, Status.Cancelled)]
        [InlineData(Status.Cancelled, Status.New)]
        public async Task Update_ServiceRequests_Returns_Ok(Status currentStatus, Status newStatus)
        {
            // Arrange
            var serviceRequestId = Guid.NewGuid();
            var expectedServiceRequest = _fixture.Build<GetServiceRequestResponse>()
                .With(sr => sr.Id, serviceRequestId).With(sr => sr.Status, currentStatus).Create();
            _serviceMock.Setup(sm => sm.GetServiceRequest(serviceRequestId)).ReturnsAsync(expectedServiceRequest);
            // Act
            var response = await _controller.Update(serviceRequestId, newStatus);
            // Verify
            Assert.IsType<OkObjectResult>(response);
        }
    }
}
