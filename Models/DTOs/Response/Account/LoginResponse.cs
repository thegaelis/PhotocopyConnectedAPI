using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotocopyConnectedAPI.Models.DTOs.Response.Account
{
    public class LoginResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public string? EmployeeAtStoreId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}