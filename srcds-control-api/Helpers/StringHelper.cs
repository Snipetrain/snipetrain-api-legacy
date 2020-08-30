using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Helpers
{
    public static class StringHelper
    {
        public static byte[] ToByteArray(this string str)
        {
            return Convert.FromBase64String(str);
        }
    }
}
