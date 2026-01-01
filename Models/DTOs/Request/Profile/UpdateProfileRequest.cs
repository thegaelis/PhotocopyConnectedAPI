using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotocopyConnectedAPI.Models.DTOs.Request.Profile
{
    public class UpdateProfileRequest
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; } = null!;
        
        [Phone(ErrorMessage = "Invalid Phone number format")]
        public string? PhoneNumber { get; set; }
    }
}