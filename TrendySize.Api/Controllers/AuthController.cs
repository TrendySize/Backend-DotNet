using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrendySize.Api.DTOs;
using Microsoft.AspNetCore.Identity;      // for UserManager
using TrendySize.Api.Models;              // for ApplicationUser
using TrendySize.Api.Services;            // for ITokenService

namespace TrendySize.Api.Controllers      // literal string — plain, predictable, most common
{
    [ApiController] //Indicates that this class is an API controller, enabling automatic model validation and response formatting    
    [Route("api/auth")] //Defines the route for the controller, using the controller's name as a placeholder
    public class AuthController : ControllerBase //Inherits ControllerBase to handle HTTP requests and responses
    {
        private readonly UserManager<ApplicationUser> _userManager; //Manages user-related operations, such as creating and retrieving users    
        private readonly ITokenService _tokenService; //Handles token generation and validation for authentication

        public AuthController(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager; //Initializes the UserManager instance
            _tokenService = tokenService; //Initializes the ITokenService instance
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict(new { message = "Email is already in use." });
            }

            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            // No token here — email isn't confirmed yet, so no access should be granted.
            // Email confirmation token generation + sending is the next piece we build.
            return Ok(new { message = "Signup successful. Please check your email to confirm your account." });
        }
    }
}
