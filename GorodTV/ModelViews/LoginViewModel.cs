using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GorodTV.Models.Requests.Auth;
using GorodTV.Pages;
using GorodTV.Services;
using GorodTV.Services.Interfaces;

namespace GorodTV.ModelViews;

public partial class LoginViewModel : ObservableObject
{
    [ObservableProperty]
    private string _username;

    [ObservableProperty]
    private string _password;

    [ObservableProperty]
    private string _errorMessage;
    
    private IAuthService _authService;

    public LoginViewModel()
    {
        _authService = new AuthService();
    }

    [RelayCommand]
    private async Task Login()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Номер договора или пароль не может быть пустым";
            return;
        }

        var result = await _authService.Login(new AuthRequest(Username, Password));
        if (!result.IsSuccess)
        {
            ErrorMessage = result.Message;
            return;
        }
        await Shell.Current.DisplayAlert("Авторизация", result.Message, "OK");
        await Shell.Current.GoToAsync($"{nameof(CategoryPage)}");
    }
}