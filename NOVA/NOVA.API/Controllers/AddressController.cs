using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NOVA.API.Common.Responses;
using NOVA.Application.Features.Address.Command;
using NOVA.Application.Features.Address.Common;
using NOVA.Application.Features.Address.Query;
namespace NOVA.API.Controllers
{
    [Route("api/v1/addresses")]
    [Authorize] 
    public class AddressController(ISender sender) : BaseApiController
    {
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<AddressResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateAddress(
            [FromBody] CreateAddressCommand command,
            CancellationToken ct)
        {
            var result = await sender.Send(command, ct);
            return HandleResult(result, CreatedEnvelope);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<AddressResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserAddresses(CancellationToken ct)
        {
            var query = new GetUserAddressesQuery();
            var result = await sender.Send(query, ct);
            return HandleResult(result, OkEnvelope);
        }
    }
}