using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using InventoryManagement.Entities;
using InventoryManagement.Configurations;

namespace InventoryManagement.Services
{
    public class JwtTokenGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtTokenGenerator(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        // Generates a JWT token for the user.
        public string GenerateToken(User user)
        {
            // Create a list of claims for the user in order to generate a token.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Create a UTF8 encoded secret key (defined in appsettings.json).
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            // Define the signing credentials using HMACSHA256 algorithm and the secret key.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create a JWT security token with the given claims, expiration time, issuer, and audience.
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwtSettings.ExpirationDays),
                signingCredentials: creds
            );

            // Serialize the JWT token to a string and return it.
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
