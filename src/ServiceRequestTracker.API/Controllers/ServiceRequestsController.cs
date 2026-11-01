using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ServiceRequestTracker.API.Requests;
using ServiceRequestTracker.API.Responses;
using ServiceRequestTracker.API.Services;
using ServiceRequestTracker.Common.Enums;
using ServiceRequestTracker.Common.Models;

namespace ServiceRequestTracker.API.Controllers
{
    [ApiController]
    [Route("api/service-requests")]
    public class ServiceRequestsController : ControllerBase
    {
        private readonly IServiceRequestsService _serviceRequestsService;

        public ServiceRequestsController(IServiceRequestsService serviceRequestsService)
        {
            _serviceRequestsService = serviceRequestsService;
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<GetServiceRequestResponse>> Get(Guid id)
        {
            var serviceRequest = await _serviceRequestsService.GetServiceRequest(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return Ok(serviceRequest);
        }

        [HttpGet]
        public async Task<ActionResult<SearchResult<GetServiceRequestResponse>>> Get(
            [FromQuery] GetServiceRequestsRequest requestsRequest, CancellationToken token)
        {
            var serviceRequests = await _serviceRequestsService.GetServiceRequests(requestsRequest, token);

            return Ok(serviceRequests);
        }

        [HttpPost]
        public async Task<ActionResult<GetServiceRequestResponse>> Create([FromBody] CreateServiceRequest request)
        {
            await _serviceRequestsService.CreateServiceRequest(request);
            return Ok();
        }

        [HttpPatch("{id:guid}/status")]
        public async Task<ActionResult> Update([Required] Guid id, [Required] Status status)
        {
            var serviceRequest = await _serviceRequestsService.GetServiceRequest(id);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            if (serviceRequest.Status == Status.Done)
            {
                return Conflict("Cannot update status of a completed service request.");
            }

            if ((serviceRequest.Status == Status.Cancelled && status != Status.New) ||
                (serviceRequest.Status == Status.New && (status != Status.InProgress && status != Status.Cancelled)) ||
                (serviceRequest.Status == Status.InProgress && (status != Status.Done && status != Status.Cancelled))
                )
            {
                return Conflict($"Cannot update status of a {serviceRequest.Status} service request to {status}");
            }

            await _serviceRequestsService.UpdateServiceRequest(id, serviceRequest.Status, status);

            return Ok(serviceRequest);
        }
    }
}
