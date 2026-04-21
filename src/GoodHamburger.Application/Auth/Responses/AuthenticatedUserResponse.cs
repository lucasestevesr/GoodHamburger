namespace GoodHamburger.Application.Auth.Responses
{
    /// <summary>
    /// Dados do usuário autenticado retornados no login.
    /// </summary>
    public sealed class AuthenticatedUserResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
    }
}
