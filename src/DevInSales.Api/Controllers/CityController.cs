using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;

        public CityController(IStateService stateService, ICityService cityService)
        {
            _stateService = stateService;
            _cityService = cityService;
        }

        [HttpGet("/api/State/{stateId}/city")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Ok",
            type: typeof(IEnumerable<ReadCity>)
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
        [SwaggerOperation(Summary = "Get cities list")]
        public async Task<IActionResult> GetCityByStateId(int stateId, string? name)
        {
            var state = await _stateService.GetById(stateId);
            if (state == null)
                return NotFound();

            var citiesList = await _cityService.GetAll(stateId, name);
            if (citiesList == null)
                return NoContent();

            return Ok(citiesList.Select(c => new ReadCity(c)).ToList());
        }

        [HttpGet("/api/State/{stateId}/city/{cityId}")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Ok",
            type: typeof(ReadCity)
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, description: "Bad Request")]
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
        [SwaggerOperation(Summary = "Get city by id")]
        public async Task<IActionResult> GetCityById(int stateId, int cityId)
        {
            var state = await _stateService.GetById(stateId);
            if (state == null)
                return NotFound();

            var city = await _cityService.GetById(cityId);
            if (city == null)
                return NotFound();

            if (state.Id != city.State.Id)
                return BadRequest();

            return Ok(new ReadCity(city));
        }

        [HttpPost("/api/State/{stateId}/city")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status201Created,
            description: "Created"
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, description: "Bad Request")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(statusCode: StatusCodes.Status404NotFound, description: "Not Found")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Add city")]
        public async Task<ActionResult> AddCity(int stateId, AddCity model)
        {
            var state = await _stateService.GetById(stateId);
            if (state == null)
                return NotFound();

            var city = await _cityService.GetAll(stateId, model.Name);
            if (city.Any())
                return BadRequest();

            var newCity = new City(stateId, model.Name);
            await _cityService.Add(newCity);

            return CreatedAtAction(
                nameof(GetCityById),
                new { stateId, cityId = newCity.Id },
                newCity.Id
            );
        }
    }
}
