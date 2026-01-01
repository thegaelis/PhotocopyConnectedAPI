using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhotocopyConnectedAPI.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateAccessToken(string userId, string email, List<string> roles);
        string GenerateRefreshToken();
        ClaimsPrincipal? ValidateToken(string token);
    }
}