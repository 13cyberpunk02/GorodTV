using GorodTV.Pages;

namespace GorodTV;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        
        Routing.RegisterRoute("loading", typeof(LoadingPage));
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("category", typeof(CategoryPage));
        Routing.RegisterRoute("category/channel", typeof(ChannelPage));
        Routing.RegisterRoute("category/channel/epg", typeof(EpgsPage));
        Routing.RegisterRoute("category/channel/epg/player", typeof(PlayerPage));
    }
}