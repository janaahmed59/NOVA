using MediatR;
using Microsoft.AspNetCore.Mvc;
using NOVA.Application.Features.Categories.Queries.GetCategoryById;
using NOVA.Application.Features.Categories.Queries.GetCategories;

namespace NOVA.API.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController(ISender sender) : BaseApiController
    {
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id, CancellationToken ct)
        {
            var query = new GetCategoryByIdQuery(id);
            var res = await sender.Send(query, ct);
            return HandleResult(res, OkEnvelope);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] int page,
            [FromQuery] int pageSize ,CancellationToken ct)
        {
            var query = new GetCategoriesQuery(page, pageSize);
            var res = await sender.Send(query, ct);
            return HandlePagedResult(res);
        }
    }
}
