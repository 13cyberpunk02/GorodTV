﻿using GorodTV.Services;
using GorodTV.Services.Interfaces;

namespace GorodTV.Pages;

public partial class LoadingPage : ContentPage
{
    private readonly IAuthService _authService;
    public LoadingPage()
    {
        InitializeComponent();
        _authService = new AuthService();
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        try
        {
            base.OnNavigatedTo(args);
            if (await _authService.CheckAuthorizationAsync())
            {
                await Shell.Current.GoToAsync($"//{nameof(CategoryPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}