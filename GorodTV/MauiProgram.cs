using CommunityToolkit.Maui;
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

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}