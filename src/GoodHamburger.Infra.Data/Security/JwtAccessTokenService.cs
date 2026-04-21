using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GoodHamburger.Application.Auth.Interfaces;
using GoodHamburger.Domain.Entities.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GoodHamburger.Infra.Data.Security
{
    public sealed class JwtAccessTokenService(IOptions<JwtOptions> options) : IAccessTokenService
    {
        private readonly JwtOptions jwtOptions = options.Value;

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(jwtOptions.ExpiresInMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int GetExpiresInSeconds()
        {
            return jwtOptions.ExpiresInMinutes * 60;
        }
    }
}
