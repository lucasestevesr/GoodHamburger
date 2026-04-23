using System.ComponentModel.DataAnnotations;

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
        public string Role { get; set; } = string.Empty;

        public bool IsActive { get; set; }

        [MinLength(7, ErrorMessage = "A senha deve ter pelo menos 7 caracteres.")]
        [RegularExpression(
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).+$",
            ErrorMessage = "A senha deve conter letra minúscula, letra maiúscula, número e caractere especial.")]
        public string? Password { get; set; }
    }
}
