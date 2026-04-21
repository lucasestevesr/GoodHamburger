using GoodHamburger.Application.Auth.Interfaces;
using GoodHamburger.Application.Auth.Requests;
using GoodHamburger.Application.Auth.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers
{
    /// <summary>
    /// Endpoints de autenticação.
    /// </summary>
    [ApiController]
    [AllowAnonymous]
    [Route("api/v1/auth")]
    public sealed class AuthController(IAuthService authService) : ControllerBase
    {
        /// <summary>
        /// Realiza o login e retorna um token JWT.
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var response = await authService.LoginAsync(request, ct);
            return Ok(response);
        }
    }
}
