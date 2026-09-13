using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TrendySize.Api.Models;
using TrendySize.Api.Services;



namespace TrendySize.Api.Services
{
    public class TokenService : ITokenService
    {
        private readonly  UserManager<ApplicationUser> _userManager;
        private readonly  IConfiguration _config;


        public TokenService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager =  userManager;
            _config =   config;
        }

        public async Task<string> GenerateJwtToken (ApplicationUser user)
        {
             var roles = await _userManager.GetRolesAsync(user);
             var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                };

                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            
        }
    }
}
