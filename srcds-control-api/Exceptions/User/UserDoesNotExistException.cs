using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Exceptions.User
{
    public class UserNotFoundException : UserException
    {
        public UserNotFoundException(string user) : base($"User not found - Username: {user}")
        {  }
    }
}
