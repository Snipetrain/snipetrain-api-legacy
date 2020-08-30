using srcds_control_api.Models;
using srcds_control_api.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateUser(User user);
        Task ChangePassword(PasswordChangeDto dto);
        Task CreateUser(User user);
    }
}
