using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Exceptions.User
{
    public class WrongPasswordException : UserException
    {
        public WrongPasswordException(string user) : base($"Wrong password - Username: {user}")
        {  }
    }
}
