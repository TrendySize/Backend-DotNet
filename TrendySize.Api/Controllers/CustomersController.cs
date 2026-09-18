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
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CustomersController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateOrFindCustomer(CreateCustomerRequest request)
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vendorGuid = Guid.Parse(vendorId!);

            var existing = await _db.Customers
                .FirstOrDefaultAsync(c => c.UserId == vendorGuid && c.PhoneNumber == request.PhoneNumber);

            if (existing != null)
            {
                return Ok(existing);
            }

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
        public async Task<IActionResult> GetCustomers()
        {
            var vendorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var vendorGuid = Guid.Parse(vendorId!);

            var customers = await _db.Customers
                .Where(c => c.UserId == vendorGuid)
                .ToListAsync();

            return Ok(customers);
        }
    }
}