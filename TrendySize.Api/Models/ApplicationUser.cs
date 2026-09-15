using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace TrendySize.Api.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsSubscriptionActive { get; set; } = false;
        public string? WhatsappNumber { get; set; } 
        public DateTime? SubscriptionExpiresAt { get; set; }        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
