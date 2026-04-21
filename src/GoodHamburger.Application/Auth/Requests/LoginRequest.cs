using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.Application.Auth.Requests
{
    /// <summary>
    /// Dados necessários para autenticar um usuário.
    /// </summary>
    public sealed class LoginRequest
    {
        /// <summary>
        /// E-mail do usuário.
        /// </summary>
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Senha do usuário.
        /// </summary>
        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; } = string.Empty;
    }
}
