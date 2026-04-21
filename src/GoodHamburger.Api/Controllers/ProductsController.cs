using GoodHamburger.Api.Security;
using GoodHamburger.Application.Products.Interfaces;
using GoodHamburger.Application.Products.Requests;
using GoodHamburger.Application.Products.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers
{
    /// <summary>
    /// Endpoints para gerenciamento de produtos.
    /// </summary>
    [ApiController]
    [Route("api/v1/products")]
    public sealed class ProductsController(IProductService productService) : ControllerBase
    {
        /// <summary>
        /// Lista os produtos do sistema.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.OrderManagement)]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(CancellationToken ct)
        {
            var products = await productService.ListAsync(ct);
            return Ok(products);
        }

        /// <summary>
        /// Obtém um produto pelo identificador.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.OrderManagement)]
        [HttpGet("{productId:guid}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid productId, CancellationToken ct)
        {
            var product = await productService.GetByIdAsync(productId, ct);
            return Ok(product);
        }

        /// <summary>
        /// Cria um novo produto.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ProductManagement)]
        [HttpPost]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request, CancellationToken ct)
        {
            var product = await productService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { productId = product.Id }, product);
        }

        /// <summary>
        /// Atualiza os dados de um produto.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ProductManagement)]
        [HttpPut("{productId:guid}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid productId, [FromBody] UpdateProductRequest request, CancellationToken ct)
        {
            var product = await productService.UpdateAsync(productId, request, ct);
            return Ok(product);
        }

        /// <summary>
        /// Atualiza o status de ativação de um produto.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ProductManagement)]
        [HttpPatch("{productId:guid}/status")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateStatus(Guid productId, [FromBody] UpdateProductStatusRequest request, CancellationToken ct)
        {
            var product = await productService.UpdateStatusAsync(productId, request, ct);
            return Ok(product);
        }

        /// <summary>
        /// Remove um produto.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.ProductManagement)]
        [HttpDelete("{productId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid productId, CancellationToken ct)
        {
            await productService.DeleteAsync(productId, ct);
            return NoContent();
        }
    }
}
