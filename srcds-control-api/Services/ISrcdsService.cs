using srcds_control_api.Models;
using srcds_control_api.Models.DTOs;
using srcds_control_api.Utilities.ServerQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Services
{
    public interface ISrcdsService
    {
        Task<List<MongoServer>> GetServersAsync();
        Task<ServerInfoResult> GetServerStatus(string hostname, int port);
    }
}
