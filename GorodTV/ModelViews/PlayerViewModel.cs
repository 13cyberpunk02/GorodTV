using CommunityToolkit.Mvvm.ComponentModel;
using GorodTV.Pages;

namespace GorodTV.ModelViews;

public partial class PlayerViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private string _channelLink;
    [ObservableProperty]
    private string _channelName;
    [ObservableProperty]
    private string _channelId;
 
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("channelLink") && query["channelLink"] is not null)
            ChannelLink = query["channelLink"] as string;

        if (query.ContainsKey("channelName") && query["channelName"] is not null)
            ChannelName = query["channelName"] as string;
        
        if (query.ContainsKey("channelId") && query["channelId"] is not null)
            ChannelId = query["channelId"] as string;
    }

    public async void GoBackToEpg()
    {
        var _parameters = new Dictionary<string, object>
        {
            { "channelLink", ChannelLink },
            { "channelName", ChannelName },
            { "channelId", ChannelId }
        };
        await Shell.Current.GoToAsync($"///{nameof(EpgsPage)}", _parameters);
    }
}