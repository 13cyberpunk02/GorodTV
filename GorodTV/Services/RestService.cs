using System.Net.Http.Json;
using System.Text.Json;
using GorodTV.Core;
using GorodTV.Models.Responses.Category;
using GorodTV.Models.Responses.Channel;
using GorodTV.Models.Responses.Epg;
using GorodTV.Models.Responses.UnixTime;
using GorodTV.Services.Interfaces;

namespace GorodTV.Services;

public class RestService : IRestService
{
    private HttpClient _httpClient;
    private readonly BaseApi _baseApi = new();
    
    public async Task<CategoriesList> GetCategoriesRequest()
    {
        _httpClient = new HttpClient();
        var sessionId = await SecureStorage.Default.GetAsync("sessionId");
        var response = await _httpClient
            .GetFromJsonAsync<CategoriesList>(_baseApi.GetCategoryRequestString(sessionId));
        if (response is null)
            return null;
        return response; 
    }

    public async Task<ChannelsList> GetChannelsRequest()
    {
        _httpClient = new HttpClient();
        var sessionId = await SecureStorage.Default.GetAsync("sessionId");
        var response = await _httpClient
            .GetFromJsonAsync<ChannelsList>(_baseApi.GetChannelsRequestString(sessionId));
        if (response is null)
            return null;
        return response;
    }

    public async Task<EpgsList> GetEpgOneDay(string startTime, string channelId)
    {
        
        using (_httpClient = new HttpClient())
        {
            try
            {
                var sessionId = await SecureStorage.Default.GetAsync("sessionId");
                HttpResponseMessage response = await _httpClient.GetAsync(_baseApi.GetEpgRequestString(startTime, channelId, sessionId));
                response.EnsureSuccessStatusCode();
                string body = await response.Content.ReadAsStringAsync();
                EpgsList epgs = JsonSerializer.Deserialize<EpgsList>(body);
                return epgs;
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }            
    }

    public async Task<UnixTime> GetUnixTimeAsync()
    {
        _httpClient = new HttpClient();
        var response = await _httpClient.GetFromJsonAsync<UnixTime>(_baseApi.GetUnixTimeRequestString);
        if (response is null)
            return null;
        return response;
    }
}