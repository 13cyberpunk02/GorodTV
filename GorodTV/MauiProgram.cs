using CommunityToolkit.Maui;
using GorodTV.ModelViews;
using GorodTV.Pages;
using GorodTV.Services;
using GorodTV.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

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
            })
            .ConfigureLifecycleEvents(events =>
            {
                events
                .AddAndroid(android => android
                .OnPause((activity) => OnPause())
                .OnResume((activity) => OnResume()));                
            });
        
        //Services
        builder.Services.AddTransient<IAuthService, AuthService>();
        builder.Services.AddTransient<IRestService, RestService>();
        
        //UIs
        builder.Services.AddSingleton<LoadingPage>();
        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddTransient<CategoryPage>();
        builder.Services.AddTransient<ChannelPage>();
        builder.Services.AddTransient<EpgsPage>();
        builder.Services.AddTransient<PlayerPage>();
        
        //ModelViews
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<CategoryViewModel>();
        builder.Services.AddSingleton<ChannelViewModel>();
        builder.Services.AddSingleton<EpgsViewModel>();
        builder.Services.AddSingleton<PlayerViewModel>();

#if DEBUG
    builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void OnPause()
    {
        // Приложение переходит в фоновый режим
        // Остановить воспроизведение видео        
        if (Shell.Current.CurrentPage is ContentPage contentPage &&
            contentPage is PlayerPage playerPage)
        {            
            playerPage.PauseVideo();
        }
    }

    private static void OnResume()
    {
        // Приложение возвращается на передний план
        // Можно возобновить воспроизведение, если нужно
        if (Shell.Current.CurrentPage is ContentPage contentPage &&
            contentPage is PlayerPage playerPage)
        {
            playerPage.PlayVideo();
        }
    }
}