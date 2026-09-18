using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TrendySize.Api.Services;

namespace TrendySize.Api.DTOs
{
    //Table: UpdateProfileRequest
    public class UpdateProfileRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? WhatsappNumber { get; set; }
    }
}
