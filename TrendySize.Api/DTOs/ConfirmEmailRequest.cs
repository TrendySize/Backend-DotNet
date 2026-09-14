using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TrendySize.Api.DTOs
{
    // DTO for confirming email requests, containing the user ID and confirmation token.
    public class ConfirmEmailRequest
    {
        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
