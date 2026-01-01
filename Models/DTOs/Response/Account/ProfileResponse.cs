using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotocopyConnectedAPI.Models.DTOs.Response.Account
{
    public class ProfileResponse: LoginResponse
    {
        public bool IsActive {get; set;}
        public string? PhoneNumber { get; set;}
        public DateTime CreatedAt { get; set;}
    }
}