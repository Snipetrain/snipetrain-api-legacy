﻿using srcds_control_api.Models;
using srcds_control_api.Models.DTOs;
using srcds_control_api.Utilities.ServerQuery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace srcds_control_api.Services
{
    public interface IRankService
    {
        Task<IEnumerable<Player>> GetRanks(int perPage);
        Task<IEnumerable<Player>> GetRanks(int perPage, string searchString);
        Task<Player> GetRank(string steamId);
    }
}
