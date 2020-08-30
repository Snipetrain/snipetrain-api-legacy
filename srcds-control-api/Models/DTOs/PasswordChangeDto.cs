using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Models.DTOs
{
    public class PasswordChangeDto
    {
        public string UserName { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public bool IsValid()
        {
            return (!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(CurrentPassword) || !string.IsNullOrEmpty(NewPassword));
        }
    }
}
