using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Models
{
    public class CorsSettings
    {
        public string[] AllowedOrigins { get; set; }
    }

    public class JwtSettings
    {
        public string Secret { get; set; }
    }
}
