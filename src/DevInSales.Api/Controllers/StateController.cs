using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class StateController : ControllerBase
    {
        private readonly IStateService _stateService;

        public StateController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpGet]
        [Authorize(Policy = "RequireUserRole")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Ok",
            type: typeof(IEnumerable<ReadState>)
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(Summary = "Get state list")]
        public async Task<IActionResult> GetAll(string? name)
        {
            var statesList = await _stateService.GetAll(name);
            if (!statesList.Any())
                return NoContent();

            return Ok(statesList.Select(s => new ReadState(s)).ToList());
        }

        [HttpGet("{stateId}")]
        [Authorize(Policy = "RequireUserRole")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Ok",
            type: typeof(ReadState)
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Not Found")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(Summary = "Get state by id")]
        public async Task<IActionResult> GetByStateId(int stateId)
        {
            var state = await _stateService.GetById(stateId);

            if (state == null)
                return NotFound();

            return Ok(new ReadState(state));
        }
    }
}
