using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhotocopyConnectedAPI.Models.DTOs.Request.Profile;
using PhotocopyConnectedAPI.Models.DTOs.Request.Account;
using PhotocopyConnectedAPI.Models.DTOs.Response.Account;
using PhotocopyConnectedAPI.Models.DTOs.Response.Common;




namespace PhotocopyConnectedAPI.Services.Interfaces
{
    public interface IAccountService
    {
        //Auth
        Task<ApiResponse<LoginResponse>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);
        Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);
        Task<ApiResponse<string>> LogoutAsync(string userId);
        
        //Profile management
        Task<ApiResponse<ProfileResponse>> GetProfileAsync(string userId);
        Task<ApiResponse<ProfileResponse>> UpdateProfileAsync(string userId, UpdateProfileRequest request);

        //Admin
        Task<ApiResponse<List<ProfileResponse>>> GetAllAccountsAsync();
        Task<ApiResponse<ProfileResponse>> GetAccountByIdAsync(string userId);



    }
}