using System.Threading.Tasks;
using RestSharp;
using RestSharp.Serialization.Xml;
using srcds_control_api.Models;

public class GameMeRest
{
    private readonly RestClient _client;
    public GameMeRest(string baseUri) {
        _client = new RestClient(baseUri);
        _client.UseDotNetXmlSerializer();
    }

    public async Task<GameME> GetGameMeAsync(string path) {
        var request = new RestRequest(path);
        return await _client.GetAsync<GameME>(request);
    }
}