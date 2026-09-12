using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


//Signup requests for ClientSide

namespace TrendySize.Api.DTOs
{
    public class SignupRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required][EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string? PhoneNumber { get; set; }
        [Required] [MinLength(8)]
        public string Password { get; set; } = string.Empty;
        
    }
}
