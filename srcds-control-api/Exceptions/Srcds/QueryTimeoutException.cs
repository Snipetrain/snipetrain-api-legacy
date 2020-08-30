using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Exceptions.Srcds
{
    public class QueryTimeoutException : Exception
    {
        public QueryTimeoutException(string msg) : base(msg)
        {  }
    }
}
