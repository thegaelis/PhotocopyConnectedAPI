using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotocopyConnectedAPI.Models.DTOs.Response.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message {get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; }
        public static ApiResponse<T> SuccessResponse (T data, string message = "Success") => new ApiResponse<T> { Success = true, Message = message, Data = data };
        public static ApiResponse<T> ErrorResponse (string message, List<string>? errors = null) => new ApiResponse<T> { Success = false, Message = message, Errors = errors ?? new List<string>() };

    }
}