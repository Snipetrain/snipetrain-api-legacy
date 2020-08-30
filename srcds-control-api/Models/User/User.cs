using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Models
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password));
        }
    }

    public static class UserRole
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";
        public const string All = "User, Admin, SuperAdmin";
    }
}
