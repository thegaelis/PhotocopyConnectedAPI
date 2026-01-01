using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotocopyConnectedAPI.Configurations
{
    public class JwtConfig
    {
        public string Secret { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int AccessTokenExpirationMinutes { get; set; } = 60;
        public int RefreshTokenExpirationDays { get; set; } = 7; // a week
        public int RefreshTokenExpirationDaysRememberMe { get; set; } = 90; // 3 months

    }
}