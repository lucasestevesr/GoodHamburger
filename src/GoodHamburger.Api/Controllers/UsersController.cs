using GoodHamburger.Api.Security;
using GoodHamburger.Application.Users.Interfaces;
using GoodHamburger.Application.Users.Requests;
using GoodHamburger.Application.Users.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers
{
    /// <summary>
    /// Endpoints para gerenciamento de usuários.
    /// </summary>
    [ApiController]
    [Route("api/v1/users")]
    public sealed class UsersController(IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Lista todos os usuários.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.UserManagement)]
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<UserResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> List(CancellationToken ct)
        {
            var users = await userService.ListAsync(ct);
            return Ok(users);
        }

        /// <summary>
        /// Obtém um usuário pelo identificador.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.UserManagement)]
        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid userId, CancellationToken ct)
        {
            var user = await userService.GetByIdAsync(userId, ct);
            return Ok(user);
        }

        /// <summary>
        /// Cria um usuário com qualquer perfil.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.UserManagement)]
        [HttpPost]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request, CancellationToken ct)
        {
            var user = await userService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { userId = user.Id }, user);
        }

        /// <summary>
        /// Cria um usuário com perfil Attendant.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.CreateAttendantManagement)]
        [HttpPost("attendants")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateAttendant([FromBody] CreateAttendantRequest request, CancellationToken ct)
        {
            var user = await userService.CreateAttendantAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { userId = user.Id }, user);
        }

        /// <summary>
        /// Atualiza os dados de um usuário.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.UserManagement)]
        [HttpPut("{userId:guid}")]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(Guid userId, [FromBody] UpdateUserRequest request, CancellationToken ct)
        {
            var user = await userService.UpdateAsync(userId, request, ct);
            return Ok(user);
        }

        /// <summary>
        /// Remove um usuário.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.UserManagement)]
        [HttpDelete("{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid userId, CancellationToken ct)
        {
            await userService.DeleteAsync(userId, ct);
            return NoContent();
        }

        /// <summary>
        /// Remove um usuário do tipo Attendant.
        /// </summary>
        [Authorize(Policy = AuthorizationPolicies.CreateAttendantManagement)]
        [HttpDelete("attendants/{userId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAttendant(Guid userId, CancellationToken ct)
        {
            await userService.DeleteAttendantAsync(userId, ct);
            return NoContent();
        }
    }
}
