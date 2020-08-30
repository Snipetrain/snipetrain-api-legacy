using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Exceptions.User
{
    public class UserAlreadyExistsException : UserException
    {
        public UserAlreadyExistsException(string user) : base($"Username Already Exists - Username: {user}")
        {  }
    }
}
