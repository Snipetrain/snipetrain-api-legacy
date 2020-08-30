using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using srcds_control_api.Exceptions.Srcds;

namespace srcds_control_api.Utilities.ServerQuery
{
    public partial class ServerQuery
    {
        public ServerQuery(IPEndPoint endPoint)
        {
            EndPoint = endPoint;
        }

        public IPEndPoint EndPoint { get; private set; }

        public async Task<ServerInfoResult> GetServerInfo()
        {
            using (var client = new UdpClient(new IPEndPoint(IPAddress.Any, 0)))
            {
                client.Connect(EndPoint);
                var requestPacket = new List<byte>();
                requestPacket.AddRange(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF});
                requestPacket.Add(0x54);
                requestPacket.AddRange(Encoding.ASCII.GetBytes("Source Engine Query"));
                requestPacket.Add(0x00);
                await client.SendAsync(requestPacket.ToArray(), requestPacket.ToArray().Length);

                var response = Task.Run(() =>
                {
                    var task = client.ReceiveAsync();

                    task.Wait(TimeoutSettings.ReceiveTimeout);

                    if (task.IsCompleted)
                        return task.Result;

                    throw new QueryTimeoutException($"Query using IP {EndPoint} timed out after {TimeoutSettings.ReceiveTimeout / 1000} seconds.");
                });

                return ServerInfoResult.Parse(response.Result.Buffer);
            }
        }


        public static class TimeoutSettings
        {
            public static int ReceiveTimeout = 10000;
        }
    }
}