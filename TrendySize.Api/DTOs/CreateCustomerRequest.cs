using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TrendySize.Api.Models;

namespace TrendySize.Api.DTOs
{

    //Table: CreateCustomerRequest
    public class CreateCustomerRequest
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public Gender Gender { get; set; }
    }
}