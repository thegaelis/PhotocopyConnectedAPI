using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PhotocopyConnectedAPI.Models.DTOs.Request.Account
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage ="Refresh token is required")]
        public string RefreshToken { get; set; } = null!;

    }
}