using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PhotocopyConnectedAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(jwtOptions =>
{
	jwtOptions.Authority = "https://{--your-authority--}";
	jwtOptions.Audience = "https://{--your-audience--}";
});

builder.Services.AddDbContext<ApplicationDbContext> (options =>
{
    options.UseSqlServer();
}
);

