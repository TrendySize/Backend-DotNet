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
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService; //Initializes the AuthController instance
        }

        //To signup a new user
        [HttpPost("signup")]
        public async Task<IActionResult> Signup(SignupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.SignupAsync(request);

            if (!result.Succeeded)
            {
                if (result.ErrorMessage != null)
                {
                    return Conflict(new { message = result.ErrorMessage });
                }
                return BadRequest(result.Errors);
            }
            return Ok(new { message = "Signup successful. Please check your email to confirm your account." });
        }

        //To Confirm email address of a user
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.ConfirmEmailAsync(request);

            if (!result.Succeeded)
            {
                if (result.ErrorMessage != null)
                {
                    return NotFound(new { message = result.ErrorMessage });
                }
                return BadRequest(result.Errors);
            }
            return Ok(new { message = "Email confirmed successfully." });
        }
    }
}
