using System.Text.Json;
using GorodTV.Core;
using GorodTV.Models.Requests.Auth;
using GorodTV.Models.Responses.Auth;
using GorodTV.Services.Interfaces;

namespace GorodTV.Services;

public class AuthService : IAuthService
{
    private HttpClient _httpClient; 
    private readonly BaseApi _baseApi = new BaseApi();
    private readonly JsonSerializerOptions serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    
    public async Task<bool> CheckAuthorizationAsync()
    {
        try
        {
            var sessionId = await SecureStorage.Default.GetAsync("sessionId");
            var userName = await SecureStorage.Default.GetAsync("username");
            var password = await SecureStorage.Default.GetAsync("password");
            var authModel = new AuthRequest(userName, password);

            if (string.IsNullOrEmpty(sessionId) && (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password)))
                return false;

            var isSessionIdValid = await CheckIsSessionIdValid(sessionId);

            if (isSessionIdValid) return true;
            else
            {
                var newSessionId = await Relogin(authModel);

                if (string.IsNullOrEmpty(newSessionId))
                    return false;
                else
                {
                    await SecureStorage.Default.SetAsync("sessionId", newSessionId);
                    await SecureStorage.Default.SetAsync("username", userName);
                    await SecureStorage.Default.SetAsync("password", password);
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); 
            return false;
        }
    }

    public async Task<AuthResponse> Login(AuthRequest model)
    {
        await SecureStorage.Default.SetAsync("username", model.Username);
        await SecureStorage.Default.SetAsync("password", model.Password);
        _httpClient = new HttpClient();
        Uri uri = new Uri(_baseApi.GetAuthRequestString(model.Username, model.Password));
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var session = JsonSerializer.Deserialize<Session>(content, serializerOptions)!;
                await SecureStorage.Default.SetAsync("sessionId", session.SessionId);
                return new AuthResponse(IsSuccess: true, Message: "Успешная авторизация");;
            }
            return new AuthResponse(IsSuccess: false, Message: "Неверный номер договора или пароль от договора");;
        }
        catch (Exception ex) 
        {
            return new AuthResponse(IsSuccess: false, Message: $"Произошла ошибка при авторизации\nОшибка:{ex.Message}");
        }
    }
    
    private async Task<string> Relogin(AuthRequest model)
    {
        _httpClient = new HttpClient(); 
        Uri uri = new Uri(string.Format(_baseApi.GetAuthRequestString(model.Username, model.Password)));
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                var session = JsonSerializer.Deserialize<Session>(content, serializerOptions)!;
                await SecureStorage.Default.SetAsync("sessionId", session.SessionId);
                return session.SessionId;
            }
            return string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }
    
    private async Task<bool> CheckIsSessionIdValid(string sessionId)
    {
        _httpClient = new HttpClient();
        Uri uri = new Uri(_baseApi.GetIsSessionValidString(sessionId));
        try
        {
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(uri);
            if (!responseMessage.IsSuccessStatusCode)
                return false;
            return true;
        }
        catch 
        {
            return false;   
        }   
    }
}