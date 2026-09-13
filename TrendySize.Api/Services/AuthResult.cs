using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TrendySize.Api.DTOs;
using TrendySize.Api.Models;
using Microsoft.AspNetCore.Mvc;


namespace TrendySize.Api.Services
{
    public class AuthResult
    {
        public bool Succeeded {get; set;}
        public string? ErrorMessage {get; set;}
        public IEnumerable<string> Errors {get; set;}

    }


}
