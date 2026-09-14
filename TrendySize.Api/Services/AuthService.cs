using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TrendySize.Api.DTOs;
using TrendySize.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace TrendySize.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //Instance of UserManager is injected into the AuthService class through its constructor. 
        //This allows the AuthService to use the UserManager to manage user accounts, 
        //such as creating new users and checking for existing users.
        public AuthService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        //Create a new user account based on the information provided in the SignupRequest object.
        public async Task<AuthResult> SignupAsync(SignupRequest request)
        {
            //Check if a user with the provided email already exists in the system.
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResult
                {
                    Succeeded = false,
                    ErrorMessage = "Email is already in use."
                };
            }
            //If the email is not already in use, a new ApplicationUser object is created with 
            //the provided information (first name, last name, email, phone number, and username).
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email
            };
            //The CreateAsync method of the UserManager is called to create the new user account with the provided password.
            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return new AuthResult
                {
                    Succeeded = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            return new AuthResult { Succeeded = true };
        }
    }
}