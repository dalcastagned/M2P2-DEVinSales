using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevInSales.Api.Utils
{
    public class GenerateJWT
    {
        public GenerateJWT() { }

        public async Task<string> Generate(
            User appUser,
            IConfiguration configuration,
            UserManager<User> userManager
        )
        {
            var key = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings:Token").Value)
            );
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var roles = await userManager.GetRolesAsync(appUser);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityTokenHandler().CreateToken(tokenDescriptor)
            );
        }
    }
}
