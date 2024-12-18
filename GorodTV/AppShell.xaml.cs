using GorodTV.Pages;

namespace GorodTV;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
        Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        Routing.RegisterRoute(nameof(CategoryPage), typeof(CategoryPage));
        Routing.RegisterRoute(nameof(ChannelPage), typeof(ChannelPage));
        Routing.RegisterRoute(nameof(EpgsPage), typeof(EpgsPage));
        Routing.RegisterRoute(nameof(PlayerPage), typeof(PlayerPage));
    }
}