using CommerceAI.API.Contracts.Products;
using CommerceAI.Application.Features.Products.CreateProduct;
using CommerceAI.Application.Features.Products.DeleteProduct;
using CommerceAI.Application.Features.Products.UpdateProduct;
using CommerceAI.Application.Queries.Products.GetProductById;
using CommerceAI.Application.Queries.Products.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CommerceAI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _sender;

        public ProductsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create(
            CreateProductCommand command,
            CancellationToken cancellationToken)
        {
            var productId= await _sender
                .Send(command,cancellationToken);

            return CreatedAtAction(
                 nameof(GetById),
                 new { id = productId },
                 new { id = productId });
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var product = await _sender.Send(
                new GetProductByIdQuery(id),
                cancellationToken);

            if (product is null)
                return NotFound();

            return Ok(product);
        }


        [HttpGet]
        public async Task<IActionResult> GetProducts(
            [FromQuery] GetProductsQuery query,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                query,
                cancellationToken);

            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(
            Guid id,
            UpdateProductRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Price,
                request.Stock,
                request.Version);

            await _sender.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(
            Guid Id,
            CancellationToken cancellationToken)
        {
            await _sender.Send(
                new DeleteProductCommand(Id),
                cancellationToken);

            return NoContent();
        }
    }
}
