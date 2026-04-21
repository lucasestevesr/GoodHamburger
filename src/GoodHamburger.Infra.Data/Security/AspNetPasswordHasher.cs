using GoodHamburger.Application.Auth.Interfaces;
using GoodHamburger.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;

namespace GoodHamburger.Infra.Data.Security
{
    public sealed class AspNetPasswordHasher : IPasswordHasher
    {
        private readonly PasswordHasher<User> passwordHasher = new();

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha é obrigatória.", nameof(password));

            return passwordHasher.HashPassword(new User(), password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            var result = passwordHasher.VerifyHashedPassword(new User(), passwordHash, password);

            return result is PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
}
