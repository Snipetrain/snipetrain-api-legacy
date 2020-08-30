using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Exceptions.User
{
    public class UserException : Exception
    {
        public UserException(string msg) : base(msg)
        {  }
    }
}
