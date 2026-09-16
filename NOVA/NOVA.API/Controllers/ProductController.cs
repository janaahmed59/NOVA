using MediatR;
using Microsoft.AspNetCore.Mvc;
using NOVA.Application.Features.Products.Queries.GetProductById;
using NOVA.Application.Features.Products.Queries.GetPRoducts;

namespace NOVA.API.Controllers
{
    [Route("api/v1/product")]
    [ApiController]
    public class ProductController(ISender sender) : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById
            ([FromRoute] int id, CancellationToken ct)
        {
            var query = new GetProductByIdQuery(id);
            var res = await sender.Send(query, ct);

            return HandleResult(res, OkEnvelope);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
            CancellationToken ct,
            [FromQuery] int page,
            [FromQuery] int pageSize
            )
        {
            var query = new GetProductQuery(page, pageSize);
            var result = await sender.Send(query, ct);

            return HandlePagedResult(result);
        }
    }
}
