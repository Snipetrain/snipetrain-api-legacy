using Microsoft.Extensions.Configuration;

using srcds_control_api.Models;
using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization.Xml;
using System.Linq;

namespace srcds_control_api.Services
{
    public class RankService : IRankService
    {
        private readonly IConfiguration _config;
        private readonly string _baseUri;
        private readonly GameMeRest _gameMeClient;

        public RankService(IConfiguration config)
        {
            _config = config;
            _gameMeClient = new GameMeRest(_config.GetSection("Endpoints").GetValue<string>("GameMe"));
        }

        public async Task<IEnumerable<Player>> GetRanks(int perPage)
        {
            var res = await _gameMeClient.GetGameMeAsync($"playerlist/tf/?limit={perPage}");
            return res.Playerlist.Player;
        }

        public async Task<IEnumerable<Player>> GetRanks(int perPage, string searchString)
        {
            var steamIDResults = await _gameMeClient.GetGameMeAsync($"playerlist/tf/uniqueid/{searchString}?limit={perPage}");
            var nameResults = await _gameMeClient.GetGameMeAsync($"playerlist/tf/name/{searchString}?limit={perPage}");

            return steamIDResults.Playerlist.Player.Concat(nameResults.Playerlist.Player);
        }

        public async Task<Player> GetRank(string steamId)
        {
            var res = await _gameMeClient.GetGameMeAsync($"playerinfo/tf/{steamId}");
            return res.Playerlist.Player[0];
        }

    }
}
