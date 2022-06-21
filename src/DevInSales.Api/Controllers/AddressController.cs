using DevInSales.Api.Dtos;
using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        private readonly IStateService _stateService;
        private readonly ICityService _cityService;

        public AddressController(
            IAddressService addressService,
            IStateService stateService,
            ICityService cityService
        )
        {
            _addressService = addressService;
            _stateService = stateService;
            _cityService = cityService;
        }

        [HttpGet]
        [Authorize(Policy = "RequireUserRole")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status200OK,
            description: "Ok",
            type: typeof(IEnumerable<ReadAddress>)
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
        [SwaggerOperation(Summary = "Get addresses list")]
        public async Task<IActionResult> GetAll(
            int? stateId,
            int? cityId,
            string? street,
            string? cep
        )
        {
            var addresses = await _addressService.GetAll(stateId, cityId, street, cep);
            if (!addresses.Any())
                return NoContent();

            return Ok(addresses.Select(a => new ReadAddress(a)).ToList());
        }

        [HttpPost("/api/state/{stateId}/city/{cityId}/address")]
        [Authorize(Policy = "RequireManagerRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status201Created, description: "Created")]
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
        [SwaggerOperation(Summary = "Get address by id")]
        public async Task<IActionResult> AddAddress(int stateId, int cityId, AddAddress model)
        {
            var state = await _stateService.GetById(stateId);
            if (state == null)
                return NotFound();

            var city = await _cityService.GetById(cityId);
            if (city == null)
                return NotFound();

            if (city.State.Id != stateId)
                return BadRequest();

            var address = new Address(
                model.Street,
                model.Cep,
                model.Number,
                model.Complement,
                cityId
            );
            await _addressService.Add(address);

            return CreatedAtAction(nameof(GetAll), new { stateId, cityId }, address.Id);
        }

        [HttpDelete("{addressId}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
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
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [SwaggerOperation(Summary = "Delete city")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var address = await _addressService.GetById(addressId);

            if (address == null)
                return NotFound();
            try
            {
                await _addressService.Delete(address);
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest();
            }
        }

        [HttpPatch("{addressId}")]
        [Authorize(Policy = "RequireManagerRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
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
        [SwaggerOperation(Summary = "Update city")]
        public async Task<IActionResult> UpdateAddress(int addressId, UpdateAddress model)
        {
            var address = await _addressService.GetById(addressId);
            if (address == null)
                return NotFound();

            if (
                model.Street == null
                && model.Cep == null
                && model.Number == null
                && model.Complement == null
            )
                return BadRequest();

            if (model.Street != null)
                address.Street = model.Street;

            if (model.Cep != null)
                address.Cep = model.Cep;

            if (model.Number != null)
                address.Number = model.Number.Value;

            if (model.Complement != null)
                address.Complement = model.Complement;

            await _addressService.Update(address);
            return NoContent();
        }
    }
}
