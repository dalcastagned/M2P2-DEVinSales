using DevInSales.Api.Dtos;
using DevInSales.Core.Entities;
using DevInSales.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DevInSales.Api.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "RequireUserRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
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
        [SwaggerOperation(Summary = "Get products by id")]
        public async Task<IActionResult> ObterProdutoPorId(int id)
        {
            var produto = await _productService.ObterProductPorId(id);
            if (produto == null)
                return NotFound();
            return Ok(produto);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireManagerRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Ok")]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, description: "Bad Request")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Put))]
        [SwaggerOperation(Summary = "Update product")]
        public async Task<IActionResult> AtualizarProduto(AddProduct model, int id)
        {
            var productOld = await _productService.ObterProductPorId(id);

            if (model == null)
                return NotFound();
            if (!ModelState.IsValid || model.Name.ToLower() == "string")
                return BadRequest(
                    "O objeto tem que ser construido com um nome e nome tem que ser diferente de string"
                );
            if (await _productService.ProdutoExiste(model.Name))
                return BadRequest("esse nome já existe na base de dados");

            productOld.AtualizarDados(model.Name, model.SuggestedPrice);

            await _productService.Atualizar();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
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
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Delete))]
        [SwaggerOperation(Summary = "Delete product")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.Delete(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("não existe"))
                    return NotFound();
                if (ex.HResult == -2146233088)
                    return BadRequest(
                        "O produto especificado não pode ser excluido, porque já está atrelado a outra tabela!"
                    );

                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { Mensagem = ex.Message }
                );
            }
        }

        [HttpGet]
        [Authorize(Policy = "RequireUserRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status200OK, description: "Ok")]
        [SwaggerResponse(statusCode: StatusCodes.Status204NoContent, description: "No Content")]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, description: "Bad Request")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Get))]
        [SwaggerOperation(Summary = "Get all products")]
        public async Task<IActionResult> GetAll(string? name, decimal? priceMin, decimal? priceMax)
        {
            try
            {
                if (priceMax < priceMin)
                    return BadRequest("O preço mínimo não pode ser maior que o preço máximo");

                var ProductList = await _productService.ObterProdutos(name, priceMin, priceMax);
                if (ProductList.Count == 0 || ProductList == null)
                    return NoContent();
                return Ok(ProductList);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new { Mensagem = ex.Message }
                );
            }
        }

        [HttpPost]
        [Authorize(Policy = "RequireManagerRole")]
        [SwaggerResponse(statusCode: StatusCodes.Status201Created, description: "Created")]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest, description: "Bad Request")]
        [SwaggerResponse(
            statusCode: StatusCodes.Status401Unauthorized,
            description: "Unauthorized"
        )]
        [SwaggerResponse(
            statusCode: StatusCodes.Status500InternalServerError,
            description: "Server Error"
        )]
        [ApiConventionMethod(typeof(DefaultApiConventions), nameof(DefaultApiConventions.Post))]
        [SwaggerOperation(Summary = "Add product")]
        public async Task<IActionResult> PostProduct(AddProduct model)
        {
            var product = new Product(model.Name, model.SuggestedPrice);

            if (await _productService.ProdutoExiste(product.Name))
                return BadRequest("Esse produto já existe na base de dados");

            var ProductId = await _productService.CreateNewProduct(product);

            return CreatedAtAction(nameof(ObterProdutoPorId), new { id = ProductId }, ProductId);
        }
    }
}
