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
        private readonly  UserManager<ApplicationUser> _userManager;//Responsible for managing user accounts, including creating users, validating credentials, and retrieving user information.
        private readonly  IConfiguration _config;//Provides access to configuration settings, such as the JWT secret key, issuer, and audience, which are used to generate the JWT token.


        //Instance of UserManager and IConfiguration is injected into the TokenService class through its constructor.
        public TokenService(UserManager<ApplicationUser> userManager, IConfiguration config)
        {
            _userManager =  userManager;
            _config =   config;
        }

        //The GenerateJwtToken method generates a JWT token for the specified ApplicationUser. It retrieves the user's roles, 
        //creates claims based on the user's information and roles, and then generates a JWT token using the configured secret key, 
        //issuer, and audience. The token is set to expire in 4 hours.
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
        //Creates a new instance of JwtSecurityToken with the specified issuer, audience, claims, expiration time, and signing credentials.
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
