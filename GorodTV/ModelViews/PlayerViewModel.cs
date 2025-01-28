using CommunityToolkit.Mvvm.ComponentModel;

namespace GorodTV.ModelViews;

public partial class PlayerViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty]
    private string _channelLink;
 
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("channelLink") && query["channelLink"] is not null)
            ChannelLink = query["channelLink"] as string;
    }
}