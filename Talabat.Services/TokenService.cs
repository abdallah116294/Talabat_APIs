using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories.Servcies;

namespace Talabat.Service
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        public TokenService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<string> CreateTokenAsync(AppUser User,UserManager<AppUser>userManager)
        {
            //Payload  
            // Private Key
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, User.Id),
                new Claim(ClaimTypes.GivenName,User.DisplayName),
                new Claim(ClaimTypes.Email,User.Email),
               
                //new Claim(ClaimTypes.Role, "User"), // Assuming a role for the user, adjust as necessary
                new Claim(ClaimTypes.Name, User.UserName)
            };
            var UserRoles = userManager.GetRolesAsync(User).Result;
            foreach (var role in UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])); // Use a secure key
            var Token = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"], // Issuer of the token
                audience: _config["JWT:ValidAudience"], // Audience for the token
                claims: claims, // Claims to include in the token
                expires: DateTime.Now.AddDays(double.Parse(_config["JWT:ExpirationInDays"])), // Token expiration time
                signingCredentials: new SigningCredentials(AuthKey, SecurityAlgorithms.HmacSha256) // Signing credentials using HMAC SHA256
                );
            return new JwtSecurityTokenHandler().WriteToken(Token); // Return the token as a string
        }
    }
}
