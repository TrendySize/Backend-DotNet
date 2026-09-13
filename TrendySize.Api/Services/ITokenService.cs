using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrendySize.Api.Models;


namespace TrendySize.Api.Services
{
    public interface ITokenService
    {
        Task<string> GenerateJwtToken(ApplicationUser user);
    }
}
