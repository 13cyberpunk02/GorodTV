using CommunityToolkit.Maui;
using GorodTV.Pages;
using GorodTV.Services;
using GorodTV.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace GorodTV;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Pacifico.ttf", "Pacifico");
                fonts.AddFont("RobotoSlab.ttf", "RobotoSlab");
                fonts.AddFont("RussoOne-Regular.ttf", "RussoOneRegular");
            });
        
        //Services
        builder.Services.AddScoped<IAuthService, AuthService>();
        
        //UIs
        builder.Services.AddTransient<LoadingPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<CategoryPage>();
        
        //ModelViews

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}