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
using TrendySize.Api.Data;

namespace TrendySize.Api.Controllers
{
    //This controller can be accessed without authentication, hence no [Authorize] attribute is applied.
    //It is for the public-facing endpoints that do not require the user to be logged in.
  
    [ApiController]
    [Route("api/public")]

        public class PublicController : ControllerBase
        {
            private readonly ApplicationDbContext _db;

            public PublicController(ApplicationDbContext db)
            {
                _db = db;
            }

            [HttpGet("v/{username}")]
            // This endpoint redirects to the vendor's WhatsApp number if the vendor exists and has an active subscription.
            public async Task<IActionResult> RedirectToWhatsApp(string username)
            {
                //Find the vendor: _userManager.FindByNameAsync(username)
                var vendor = await _db.Users.FirstOrDefaultAsync(u => u.UserName == username);
                //if null, return NotFound()
                if (vendor == null)
                {
                    return NotFound("Vendor not found.");
                }
                //Check subscription: if vendor.IsSubscriptionActive is false, return something 
                //like BadRequest(new { message = "This vendor's account is currently inactive." })
                if (!vendor.IsSubscriptionActive)
                {
                    return BadRequest(new { message = "This vendor's account is currently inactive." });
                }
                //If vendor.WhatsappNumber is null/empty, return BadRequest (nothing to redirect to)
                if(string.IsNullOrEmpty(vendor.WhatsappNumber))
                {
                    return BadRequest(new { message = "This vendor has not provided a WhatsApp number." });
                }
                //Build the WhatsApp URL: $"https://wa.me/{vendor.WhatsappNumber}?text=Hi! I'd like to get my measurements taken for an order."
                var whatsappUrl = $"https://wa.me/{vendor.WhatsappNumber}?text=Hi! I'd like to get my measurements taken for an order.";
                //Return Redirect(waUrl)
                return Redirect(whatsappUrl);
                
            }
        }
    
}
