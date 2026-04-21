using System.ComponentModel.DataAnnotations;
using GoodHamburger.Domain.Entities.Auth;

namespace GoodHamburger.Application.Users.Requests
{
    public sealed class UpdateUserRequest
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido.")]
        [MaxLength(150, ErrorMessage = "O e-mail deve ter no máximo 150 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O perfil é obrigatório.")]
        public UserRole Role { get; set; }

        public bool IsActive { get; set; }

        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string? Password { get; set; }
    }
}
