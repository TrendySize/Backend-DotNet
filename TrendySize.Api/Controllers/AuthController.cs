using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // for [Authorize] attribute
using TrendySize.Api.DTOs;
using Microsoft.AspNetCore.Identity;      // for UserManager
using TrendySize.Api.Models;              // for ApplicationUser
using TrendySize.Api.Services;            // for ITokenService
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Claims;


namespace TrendySize.Api.Controllers      // literal string — plain, predictable, most common
{
    [ApiController] //Indicates that this class is an API controller, enabling automatic model validation and response formatting    
    [Route("api/auth")] //Defines the route for the controller, using the controller's name as a placeholder
    public class AuthController : ControllerBase //Inherits ControllerBase to handle HTTP requests and responses
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService; //Initializes the AuthController instance
            _userManager = userManager; //Initializes the UserManager instance
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

        //To build Login endpoint for user
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            //check if the model state is valid, if not return a bad request with the model state errors
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _authService.LoginAsync(request);
            //check if the login attempt was successful, if not return an unauthorized response with the error message
            if (!result.Succeeded)
            {
                return Unauthorized(new { message = result.ErrorMessage });
            }
            return Ok(new { token = result.Token });
        }
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(new { message = "Test endpoint is working!" });
        }

        //Building a secured login endpoint for vendors
        [HttpGet("me")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GetProfile()
        {
            //Assess the user ID from the JWT token claims and retrieve the corresponding user from the database using the UserManager service. If the user is not found, return a NotFound response; otherwise, return the user's profile information in an Ok response.
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
            {
                return NotFound();
            }
            //Return the user's profile information in an Ok response, including first name, last name, email, and WhatsApp number.
            return Ok(new
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email,
                whatsappNumber = user.WhatsappNumber
            });
        }

        [HttpPut("me")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileRequest request)
        {
            // Get userId from claims (same line you already know from GetProfile)
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            // Find the user with _userManager.FindByIdAsync
            var user = await _userManager.FindByIdAsync(userId!);
            // If null, return NotFound()
            if (user == null)
            {
                return NotFound();
            }
            // For each of FirstName, LastName, WhatsappNumber: if the request value isn't null, update the user's property
            if (request.FirstName != null)
            {
                user.FirstName = request.FirstName;
            }
            if (request.LastName != null)
            {
                user.LastName = request.LastName;
            }
            if (request.WhatsappNumber != null)
            {
                user.WhatsappNumber = request.WhatsappNumber;   
            }
            //Save the changes using _userManager.UpdateAsync(user) — a new method you haven't used yet
            var result = await _userManager.UpdateAsync(user);
            // Return Ok with a success message
            if (result.Succeeded)
            {
                return Ok(new { message = "Profile updated successfully." });
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }
    }
}
