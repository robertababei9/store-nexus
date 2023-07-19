using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Services
{
    public class JwtGenerator
    {


        public string GetToken(string secretKey, User user)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(secretKey);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            SecurityToken jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(jwtToken);

            return token;
        }
    }
}
