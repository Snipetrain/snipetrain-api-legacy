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

namespace srcds_control_api.Services
{
    public class SrcdsService : ISrcdsService
    {
        private readonly IMongoCollection<MongoServer> _servers;
        private readonly IMongoCollection<MongoServer> _admins;
        private readonly IConfiguration _config;

        public SrcdsService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("SrcdControlDb"));
            var database = client.GetDatabase("SrcdControl");

            _servers = database.GetCollection<MongoServer>("servers");
            _admins = database.GetCollection<MongoServer>("admins");

            _config = config;
        }

        public async Task<List<MongoServer>> GetServersAsync()
        {
            return (await _servers.FindAsync(s => true)).ToList();
        }

        public async Task<ServerInfoResult> GetServerStatus(string host, int port)
        {
            try
            {
                var hostEntry = Dns.GetHostEntry(host);

                if (hostEntry.AddressList.Length < 1)
                {
                    throw new Exception($"Hostname did not resolve.");
                }

                var query = new ServerQuery(new IPEndPoint(hostEntry.AddressList[0], port));
                var res = await query.GetServerInfo();

                return res;
            }
            catch (Exception)
            {
                throw;
            }

        }

    }
}
