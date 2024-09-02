using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.Entities.Constants;
using TaskManager.Entities.DB;

namespace TaskManager.BL.Auth
{
    public static class JwtProvider
    {
        public static string GenerateToken(UserModel user, IConfiguration config)
        {
            var key = config["Jwt:Key"];
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)), SecurityAlgorithms.HmacSha256);

            Claim[] claims = new[] { new Claim(CustomClaims.UserId, user.Id.ToString()) };

            var token = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenValue;
        }
    }
}
