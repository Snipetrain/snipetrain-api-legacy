using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using srcds_control_api.Exceptions.User;
using srcds_control_api.Models;
using srcds_control_api.Models.DTOs;
using srcds_control_api.Utilities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using srcds_control_api.Utilities.ServerQuery;
using RestSharp;
using RestSharp.Serializers;
using RestSharp.Serialization.Xml;

namespace srcds_control_api.Services
{
    public class RankService : IRankService
    {
        private readonly IConfiguration _config;
        private readonly string _baseUri;

        public RankService(IConfiguration config)
        {
            _config = config;
            _baseUri = _config.GetSection("Endpoints").GetValue<string>("GameMe");
        }

        public async Task<IEnumerable<Player>> GetRanks(int perPage)
        {
            try
            {
                var client = new RestClient(_baseUri);
                client.UseDotNetXmlSerializer();
                var request = new RestRequest($"playerlist/tf?limit={perPage}");
                var res = await client.GetAsync<GameME>(request);

                return res.Playerlist.Player;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<Player> GetRank(string steamId)
        {
            try
            {
                var client = new RestClient(_baseUri);
                client.UseDotNetXmlSerializer();
                var request = new RestRequest($"playerinfo/tf/{steamId}");
                var res = await client.GetAsync<GameME>(request);

                return res.PlayerInfo.Player[0];
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
