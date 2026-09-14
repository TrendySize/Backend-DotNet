using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrendySize.Api.Models;


namespace TrendySize.Api.Services
{
    public interface ITokenService
    {
        // The ITokenService interface defines the contract for token-related operations, 
        //specifically generating JWT tokens for authenticated users.
        Task<string> GenerateJwtToken(ApplicationUser user);
    }
}
