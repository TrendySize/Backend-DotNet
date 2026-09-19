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
    [ApiController]
    [Route("api/measurements")]
    [Authorize(AuthenticationSchemes = "Bearer")]

    public class MeasurementsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        // Constructor to inject the database context.
        public MeasurementsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost("generate-link")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> GenerateLinkRequest(GenerateLinkRequest request)
        {
            {
            // Get vendorId from claims (same line you've used twice now)
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //Find the customer: _db.Customers.FirstOrDefaultAsync(c => c.Id == request.CustomerId && c.UserId == vendorGuid)
            var customer = await _db.Customers.FirstOrDefaultAsync(c=> c.Id == request.CustomerId && c.UserId == Guid.Parse(vendorId!));
            //if null, return NotFound() — this also protects against one vendor generating a link for another vendor's customer
            if (customer == null)
            {
                return NotFound("Customer not found or does not belong to the authenticated vendor.");
            }
            //Generate a token: Guid.NewGuid().ToString("N") — a new concept, explained below
            var token = Guid.NewGuid().ToString("N");
            //Create a new Measurement
            var measurement = new Measurement
            {
                ClientId = customer.Id,
                LinkToken = token,
                OrderReference = request.OrderReference,
                Status = MeasurementStatus.Pending
            };
            //Add it to _db.Measurements, SaveChangesAsync()
            _db.Measurements.Add(measurement);
            await _db.SaveChangesAsync();
            //Return Ok with the full link: $"https://localhost:3000/measure?token={token}"
            var fullLink = $"https://localhost:3000/measure?token={token}";
            return Ok(new { Link = fullLink });
            }
        
        }

    }
}
