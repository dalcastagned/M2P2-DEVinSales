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
    public class SaleProductController : ControllerBase
    {
        private readonly ISaleProductService _saleProductService;

        public SaleProductController(ISaleProductService saleProductService)
        {
            _saleProductService = saleProductService;
        }

        // Endpoint criado apenas para servir como path do POST {saleId}/item
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("saleById/item")]
        public async Task<IActionResult> GetSaleProductById(int saleProductId)
        {
            var id = await _saleProductService.GetSaleProductById(saleProductId);
            if (id == null)
                return NotFound();

            return Ok(id);
        }

        [HttpPost("{saleId}/item")]
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
        [SwaggerOperation(Summary = "Add product to sale")]
        public async Task<IActionResult> CreateSaleProduct(
            int saleId,
            SaleProductRequest saleProduct
        )
        {
            try
            {
                if (saleProduct.ProductId <= 0)
                    return BadRequest();

                if (saleProduct.Amount == null)
                    saleProduct.Amount = 1;

                var id = await _saleProductService.CreateSaleProduct(saleId, saleProduct);
                return CreatedAtAction(nameof(GetSaleProductById), new { saleProductId = id }, id);
            }
            catch (ArgumentException ex)
            {
                if (ex.Message.Contains("não encontrado."))
                    return NotFound();

                if (ex.Message.Contains("não podem ser negativos."))
                    return BadRequest();

                return BadRequest();
            }
        }
    }
}
