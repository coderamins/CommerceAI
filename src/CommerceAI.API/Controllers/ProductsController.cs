using CommerceAI.Application.Features.Products.CreateProduct;
using CommerceAI.Application.Queries.Products.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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


    }
}
