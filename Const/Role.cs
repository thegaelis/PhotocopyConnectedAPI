using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotocopyConnectedAPI.Const
{
    public static class RoleConstants
    {
        public const string Customer = "Customer";           // Khách hàng gửi tài liệu in
        public const string StoreOwner = "StoreOwner";       // Chủ cửa hàng photocopy
        public const string StoreEmployee = "StoreEmployee"; // Nhân viên cửa hàng photocopy
        public const string Admin = "Admin";                 // Quản lý trang web (Admin)
    }
}