using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PhotocopyConnectedAPI.Models.Entity;
using PhotocopyConnectedAPI.Services.Interfaces;
using PhotocopyConnectedAPI.Models.DTOs.Request.Profile;
using PhotocopyConnectedAPI.Models.DTOs.Request.Account;
using PhotocopyConnectedAPI.Models.DTOs.Response.Account;
using PhotocopyConnectedAPI.Models.DTOs.Response.Common;
using PhotocopyConnectedAPI.Const;
using PhotocopyConnectedAPI.Configurations;
using Microsoft.EntityFrameworkCore;

namespace PhotocopyConnectedAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<Account> _userManager;
        private readonly SignInManager<Account> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtService _jwtManager;
        private readonly JwtConfig _jwtConfig;

        public AccountService(
            UserManager<Account> userManager,
            SignInManager<Account> signInManager,
            RoleManager<IdentityRole> roleManager,
            JwtService jwtManager,
            JwtConfig jwtConfig
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtManager = jwtManager;
            _jwtConfig = jwtConfig;
        }
        //Auth
        public async Task<ApiResponse<LoginResponse>> RegisterAsync(RegisterRequest request)
        {
            // Search the existing user by user Email
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return ApiResponse<LoginResponse>.ErrorResponse(
                    "Registration failed", 
                    new List<string> {"Email is already registered. Please log in instead."}
                );

            }
            // Initialize the account model
            var account = new Account
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = false,
            };
            // Wait for the account to be created
            var result = await _userManager.CreateAsync(account, request.Password);
            // If not succeed => show error
            if (!result.Succeeded)
            {
                return ApiResponse<LoginResponse>.ErrorResponse(
                    "Registration failed", 
                    new List<string> {"Unable to create account. Please try again later."}
                ); 
            }
            // If the account did not assigned customer role
            if (!await _roleManager.RoleExistsAsync(RoleConstants.Customer))
            {
                // Create customer role for user
                await _roleManager.CreateAsync(new IdentityRole(RoleConstants.Customer));
            }
 
            await _userManager.AddToRoleAsync(account, RoleConstants.Customer);

            // Return response for user
            var roles = await _userManager.GetRolesAsync(account);
            var response = new LoginResponse
            {
                UserId = account.Id,
                Email = account.Email!,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Roles = roles.ToList(),
                EmployeeAtStoreId = account.EmployeeAtStoreId
            }; 
            return ApiResponse<LoginResponse>.SuccessResponse(response, "Registration succeeded");
        }
        public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
        {
            // 1. Find user
            var user = await _userManager.FindByEmailAsync(
                _userManager.NormalizeEmail(request.Email)
            );
            if (user == null)
            {
                return ApiResponse<LoginResponse>.ErrorResponse("Login Failed", new List<string> {"Invalid email or password"});
            }
            // 2. Check password
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                return ApiResponse<LoginResponse>.ErrorResponse("Login Failed", new List<string> {"Invalid email or password"});
            }

            // 3. Retrieve user role
            var roles = await _userManager.GetRolesAsync(user);

            var accessToken = _jwtManager.GenerateAccessToken(user.Id, user.Email!, roles.ToList());
            var refreshToken = _jwtManager.GenerateRefreshToken();

            var expirationDays = _jwtConfig.RefreshTokenExpirationDays;
            if (request.RememberMe == true)
            {
                expirationDays = _jwtConfig.RefreshTokenExpirationDaysRememberMe;
            }
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(expirationDays);

            await _userManager.UpdateAsync(user);

            var response = new LoginResponse
            {
                UserId = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList(),
                EmployeeAtStoreId = user.EmployeeAtStoreId,
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return ApiResponse<LoginResponse>.SuccessResponse(response, "Login successful");
        }
        public async Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.RefreshToken == request.RefreshToken);
            if (user == null)
            {
                return ApiResponse<LoginResponse>.ErrorResponse("Invalid refresh token", new List<string> {"The provided refresh token is invalid"});
            }

            if (user.RefreshTokenExpiryTime < DateTime.UtcNow)
            {
                return ApiResponse<LoginResponse>.ErrorResponse("Refesh token expired", new List<string> {"The provided refresh token is expired"});
            }

            var roles = await _userManager.GetRolesAsync(user);
            
            var newAccessToken = _jwtManager.GenerateAccessToken(user.Id, user.Email!, roles.ToList());
            var newRefreshToken = _jwtManager.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpirationDays);
            
            await _userManager.UpdateAsync(user);

            var response = new LoginResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList(),
                EmployeeAtStoreId = user.EmployeeAtStoreId,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

            return ApiResponse<LoginResponse>.SuccessResponse(response, "Token refreshed successfully");

        }
        public async Task<ApiResponse<string>> LogoutAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<string>.ErrorResponse("User not found", new List<string> {"Unable to log out"});
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await _userManager.UpdateAsync(user);

            await _signInManager.SignOutAsync();

            return ApiResponse<string>.SuccessResponse("Logged out successfuly", "Logged out");

        }
        
        //Profile management
        public async Task<ApiResponse<ProfileResponse>> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<ProfileResponse>.ErrorResponse("User not found", new List<string> {"The requested user does not exist"});
            }
            var roles = await _userManager.GetRolesAsync(user);
            var response = new ProfileResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList(),
                IsActive = user.IsActive,
                EmployeeAtStoreId = user.EmployeeAtStoreId,
                CreatedAt = user.CreatedDate
            };
            return ApiResponse<ProfileResponse>.SuccessResponse(response);
        }
        public async Task<ApiResponse<ProfileResponse>> UpdateProfileAsync(string userId, UpdateProfileRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<ProfileResponse>.ErrorResponse("User not found", new List<string> {"The requested user does not exist"});
            }

            var roles = await _userManager.GetRolesAsync(user);
            
            if (request.FirstName != null && request.FirstName != user.FirstName)
            {
                user.FirstName = request.FirstName;
            }
            if (request.LastName != null && request.LastName != user.LastName)
            {
                user.LastName = request.LastName;
            }
            if (request.PhoneNumber != null)
            {
                user.PhoneNumber = request.PhoneNumber;
            }

            await _userManager.UpdateAsync(user);

            var response = new ProfileResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = roles.ToList(),
                IsActive = user.IsActive,
                EmployeeAtStoreId = user.EmployeeAtStoreId,
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(response, "Profile updated successfuly");
        }

        //Admin
        public async Task<ApiResponse<List<ProfileResponse>>> GetAllAccountsAsync()
        {
            var user = await _userManager.Users.ToListAsync();
            var responseList = new List<ProfileResponse>();
            
            foreach (var account in user)
            {
                var roles = await _userManager.GetRolesAsync(account);
                responseList.Add(new ProfileResponse
                {
                    UserId = account.Id,
                    Email = account.Email!,
                    FirstName = account.FirstName!,
                    LastName = account.LastName!,
                    IsActive = account.IsActive,
                    Roles = roles.ToList(),
                    CreatedAt = account.CreatedDate
                });
            }
            return ApiResponse<List<ProfileResponse>>.SuccessResponse(
                responseList, 
                $"Found {responseList.Count} in the record "
            );
        }
        public async Task<ApiResponse<ProfileResponse>> GetAccountByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<ProfileResponse>.ErrorResponse(
                    "User not found",
                    new List<string> { "The requested user does not exist" }
                );
            }

            var roles = await _userManager.GetRolesAsync(user);
            var response = new ProfileResponse
            {
                UserId = user.Id,
                Email = user.Email!,
                FirstName = user.FirstName!,
                LastName = user.LastName!,
                PhoneNumber = user.PhoneNumber,
                Roles = roles.ToList(),
                IsActive = user.IsActive,
                EmployeeAtStoreId = user.EmployeeAtStoreId,
                CreatedAt = user.CreatedDate
            };

            return ApiResponse<ProfileResponse>.SuccessResponse(response);
        }

    };
}