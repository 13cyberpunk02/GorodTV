using GorodTV.Models.Requests.Auth;
using GorodTV.Models.Responses.Auth;

namespace GorodTV.Services.Interfaces;

public interface IAuthService
{
    Task<bool> CheckAuthorizationAsync();
    Task<AuthResponse> Login(AuthRequest model);
}