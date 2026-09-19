using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TrendySize.Api.Data;
using TrendySize.Api.DTOs;
using TrendySize.Api.Models;

namespace TrendySize.Api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    // This controller handles customer-related operations for authenticated vendors.
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        // Constructor to inject the database context.
        public CustomersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        // This endpoint creates a new customer or finds an existing one based on the provided phone number.
        public async Task<IActionResult> CreateOrFindCustomer(CreateCustomerRequest request)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the vendor's unique identifier from the authenticated user's claims.
            var vendorGuid = Guid.Parse(vendorId!); // Parse the vendor's identifier to a GUID.

            var existing = await _db.Customers  //Get the existing customer from the database based on the vendor's ID and the provided phone number.
                .FirstOrDefaultAsync(c => c.UserId == vendorGuid && c.PhoneNumber == request.PhoneNumber);
            
            // If an existing customer is found, return it; otherwise, create a new customer with the provided details and save it to the database.
            if (existing != null)
            {
                return Ok(existing);
            }

            // Create a new customer with the provided details and save it to the database.
            var customer = new Customer
            {
                UserId = vendorGuid,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender
            };

            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return Ok(customer);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        // This endpoint retrieves all customers associated with the authenticated vendor.
        public async Task<IActionResult> GetCustomers()
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the vendor's unique identifier from the authenticated user's claims.
            var vendorGuid = Guid.Parse(vendorId!); // Parse the vendor's identifier to a GUID.

            // Retrieve all customers associated with the authenticated vendor from the database.
            var customers = await _db.Customers
                .Where(c => c.UserId == vendorGuid)
                .ToListAsync();

            return Ok(customers);
        }
    }
}