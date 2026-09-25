using MediatR;
using Microsoft.AspNetCore.Mvc;
using NOVA.API.Common.Responses;
using NOVA.Application.Features.User.Command.Login;
using NOVA.Application.Features.User.Command.RefreshTokens;
using NOVA.Application.Features.User.Command.Register;
using NOVA.Domain.Common.Results;

namespace NOVA.API.Controllers
{
    [Route("api/v1/auth")]
    [ApiController]
    public class AuthController(ISender sender) : BaseApiController
    {
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]

        public async Task<IActionResult> Register([FromBody] RegisterCommand request)
        {
            var command = new RegisterCommand(request.FullName, request.Email, request.Password);
            var result = await sender.Send(command);

            return HandleResult(result, OkEnvelope);
        }
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Login(
            [FromBody] LoginCommand command,
            CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);
            return HandleResult(result, OkEnvelope);
        }
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ApiResponse<Unit>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Refresh([FromBody] RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);

            return HandleResult(result, OkEnvelope);
        }



    }
}
    