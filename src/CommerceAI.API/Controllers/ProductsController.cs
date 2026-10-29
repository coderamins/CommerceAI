using CommerceAI.API.Contracts.Products;
using CommerceAI.Application.Features.Products.CreateProduct;
using CommerceAI.Application.Features.Products.DeleteProduct;
using CommerceAI.Application.Features.Products.UpdateProduct;
using CommerceAI.Application.Queries.Products.GetProductById;
using CommerceAI.Application.Queries.Products.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

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
            var productId = await _sender
                .Send(command, cancellationToken);

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

            Response.Headers.ETag = $"\"{product.Version}\"";

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
            var ifMatch = Request.Headers["If-Match"].ToString().Trim();

            if (string.IsNullOrWhiteSpace(ifMatch))
            {
                return StatusCode(
                    StatusCodes.Status428PreconditionRequired,
                    new ProblemDetails
                    {
                        Status = StatusCodes.Status428PreconditionRequired,
                        Title = "Precondition required.",
                        Detail = "Provide the product ETag in the if-Match header"
                    });
            }

            if (ifMatch.Length < 3 ||
                ifMatch[0] != '"' ||
                ifMatch[^1] != '"' ||
                ifMatch.Contains(',') ||
                !uint.TryParse(
                    ifMatch.AsSpan(1, ifMatch.Length - 2),
                    NumberStyles.None,
                    CultureInfo.InvariantCulture,
                    out var expectedVersion))
            {
                return BadRequest(new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Invalid If-Match header.",
                    Detail = "Provide one strong ETag in the format \"12345\"."
                });
            }

            var command = new UpdateProductCommand(
                id,
                request.Name,
                request.Price,
                request.Stock,
                expectedVersion);

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
