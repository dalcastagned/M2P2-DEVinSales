using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/sales/")]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpGet("{saleId}")]
        [Authorize(Policy = "RequireUserRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Ok")]
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
        [SwaggerOperation(Summary = "Get sales")]
        public async Task<IActionResult> GetSaleById(int saleId)
        {
            var sale = await _saleService.GetSaleById(saleId);
            if (sale == null)
                return NotFound();

            return Ok(sale);
        }

        [HttpGet("/api/user/{userId}/sales")]
        [Authorize(Policy = "RequireUserRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Ok")]
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
        [SwaggerOperation(Summary = "Get sales by user")]
        public async Task<IActionResult> GetSalesBySellerId(int? userId)
        {
            var sales = await _saleService.GetSaleBySellerId(userId);
            if (sales.Count == 0)
                return NoContent();
            return Ok(sales);
        }

        [HttpGet("/api/user/{userId}/buy")]
        [Authorize(Policy = "RequireUserRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Ok")]
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
        [SwaggerOperation(Summary = "Get buies by user")]
        public async Task<IActionResult> GetSalesByBuyerId(int? userId)
        {
            var sales = await _saleService.GetSaleByBuyerId(userId);
            if (sales.Count == 0)
                return NoContent();
            return Ok(sales);
        }

        [HttpPost("/api/user/{userId}/sales")]
        [Authorize(Policy = "RequireManagerRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Ok")]
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
        [SwaggerOperation(Summary = "Add sales")]
        public async Task<IActionResult> CreateSaleBySellerId(
            int userId,
            SaleBySellerRequest saleRequest
        )
        {
            try
            {
                Sale sale = saleRequest.ConvertToEntity(userId);
                var id = await _saleService.CreateSaleByUserId(sale);
                return CreatedAtAction(nameof(GetSaleById), new { saleId = id }, id);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.ParamName);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPatch("{saleId}/product/{productId}/price/{unitPrice}")]
        [Authorize(Policy = "RequireManagerRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Ok")]
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
        [SwaggerOperation(Summary = "Update price")]
        public async Task<IActionResult> UpdateUnitPrice(
            int saleId,
            int productId,
            decimal unitPrice
        )
        {
            try
            {
                await _saleService.UpdateUnitPrice(saleId, productId, unitPrice);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest();
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPatch("{saleId}/product/{productId}/amount/{amount}")]
        [Authorize(Policy = "RequireManagerRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Ok")]
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
        [SwaggerOperation(Summary = "Update quantity of products")]
        public async Task<IActionResult> UpdateAmount(int saleId, int productId, int amount)
        {
            try
            {
                await _saleService.UpdateAmount(saleId, productId, amount);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName.Equals("saleId") || ex.ParamName.Equals("productId"))
                    return NotFound(ex.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/user/{userId}/buy")]
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
        [SwaggerOperation(Summary = "Add buy")]
        public async Task<IActionResult> CreateSaleByBuyerId(
            int userId,
            SaleByBuyerRequest saleRequest
        )
        {
            try
            {
                Sale sale = saleRequest.ConvertToEntity(userId);
                var id = await _saleService.CreateSaleByUserId(sale);
                return CreatedAtAction(nameof(GetSaleById), new { saleId = id }, id);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.ParamName);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{saleId}/deliver")]
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
        [SwaggerOperation(Summary = "Add delivery")]
        public async Task<IActionResult> CreateDeliveryForASale(
            int saleId,
            DeliveryRequest deliveryRequest
        )
        {
            try
            {
                if (deliveryRequest.AddressId <= 0)
                    return BadRequest("AddressId não pode ser menor ou igual a zero e nulo.");

                if (deliveryRequest.DeliveryForecast == DateTime.MinValue)
                    deliveryRequest.DeliveryForecast = DateTime.Now.AddDays(7).ToUniversalTime();

                if (deliveryRequest.DeliveryForecast < DateTime.Now.ToUniversalTime())
                    return BadRequest("Data e hor�rio n�o podem ser anterior ao atual.");

                Delivery delivery = deliveryRequest.ConvertToEntity(saleId);

                int id = await _saleService.CreateDeliveryForASale(delivery);

                return CreatedAtAction(nameof(GetDeliveryById), new { deliveryId = id }, id);
            }
            catch (ArgumentException ex)
            {
                if (ex.ParamName.Equals("saleId") || ex.ParamName.Equals("AddressId"))
                    return NotFound(ex.Message);

                return BadRequest();
            }
        }

        //Endpoint criado apenas para servir como caminho do POST {saleId}/deliver
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("/api/delivery/{deliveryId}")]
        public async Task<IActionResult> GetDeliveryById(int deliveryId)
        {
            Delivery delivery = await _saleService.GetDeliveryById(deliveryId);
            if (delivery == null)
                return NoContent();
            return Ok(delivery);
        }
    }
}
