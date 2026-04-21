namespace GoodHamburger.Application.Auth.Responses
{
    /// <summary>
    /// Resultado do login com token de acesso e dados do usuário autenticado.
    /// </summary>
    public sealed class AuthResponse
    {
        public string AccessToken { get; set; } = string.Empty;

        public int ExpiresIn { get; set; }

        public AuthenticatedUserResponse User { get; set; } = new();
    }
}
