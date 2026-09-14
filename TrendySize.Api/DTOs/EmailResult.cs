using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TrendySize.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace TrendySize.Api.DTOs
{
    // The EmailResult class represents the result of an email operation, indicating 
    //whether it succeeded and providing any error messages or details.
    public class EmailResult
    {
        public bool Succeeded {get; set; }
        public string? ErrorMessage {get; set; }
        public IEnumerable<string>? Errors {get; set; }
    }
    
}
