using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using TrendySize.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace TrendySize.Api.DTOs
{
    public class LoginResult : AuthResult
    {
       public string? Token {get; set; }
    }
}
