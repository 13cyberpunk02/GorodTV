using CommunityToolkit.Maui;
using GorodTV.ModelViews;
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
        builder.Services.AddScoped<IRestService, RestService>();
        
        //UIs
        builder.Services.AddTransient<LoadingPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<CategoryPage>();
        builder.Services.AddTransient<ChannelPage>();
        builder.Services.AddTransient<EpgsPage>();
        builder.Services.AddTransient<PlayerPage>();
        
        //ModelViews
        builder.Services.AddScoped<LoginViewModel>();
        builder.Services.AddScoped<CategoryViewModel>();
        builder.Services.AddScoped<ChannelViewModel>();
        builder.Services.AddScoped<EpgsViewModel>();
        builder.Services.AddScoped<PlayerViewModel>();  
        

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}