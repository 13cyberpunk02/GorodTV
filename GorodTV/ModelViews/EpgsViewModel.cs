using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GorodTV.Models.Responses.Epg;
using GorodTV.Models.Responses.OnlineStream;
using GorodTV.Pages;
using GorodTV.Services;
using GorodTV.Services.Interfaces;

namespace GorodTV.ModelViews;

public partial class EpgsViewModel : ObservableObject, IQueryAttributable
{
    [ObservableProperty] 
    private string _channelId;
    [ObservableProperty] 
    private string _channelLink;
    [ObservableProperty] 
    private string _channelName;
    [ObservableProperty] 
    private OnlineStream _onlineStream;
    [ObservableProperty] 
    private Dictionary<int, string> _timeStamps;
    [ObservableProperty] 
    private ObservableCollection<Epg> _epgs;
    [ObservableProperty]
    private Epg _selectedEpg;
    
    private readonly IRestService _restService;
    public IAsyncRelayCommand LoadEpgsCommand { get; }
    public IAsyncRelayCommand StartLiveBroadcastCommand { get; }

    public EpgsViewModel()
    {
        Epgs = new ObservableCollection<Epg>();
        TimeStamps = new Dictionary<int, string>();
        _restService = new RestService();
        LoadEpgsCommand = new AsyncRelayCommand(LoadEpgsAsync);
        StartLiveBroadcastCommand = new AsyncRelayCommand(StartLiveBroadcastAsync);
    }


    private async Task LoadEpgsAsync()
    {
        var unixTime = await _restService.GetUnixTimeAsync();
        var response = await _restService.GetEpgOneDay(unixTime.Unixtime, ChannelId);

        if (response?.Epgs != null)
        {
            var sortedEpgs = response.Epgs.OrderBy(e => UnixTimeStampToDateTime(double.Parse(e.Start_Time))).ToList();

            OnlineStream = new OnlineStream(
                Name: ChannelName,
                Link: ChannelLink,
                Description: "Прямой эфир");

            foreach (var epg in sortedEpgs)
            {
                if(!TimeStamps.ContainsKey(int.Parse(epg.Id)))
                    TimeStamps.Add(int.Parse(epg.Id), epg.Start_Time);  
                
                if(DateTime.Now < UnixTimeStampToDateTime(double.Parse(epg.Start_Time)))
                    break;
                Epgs.Add(epg with { Start_Time = GetNormalTime(epg.Start_Time) });
            }
        }
    }

    private async Task StartLiveBroadcastAsync()
    {
        if (OnlineStream is null)
            return;
        var link = OnlineStream.Link.Replace("%TIMESTAMP%", "0");
        var parameters = new Dictionary<string, object>
        {
            { "channelLink", link },
            { "channelName", OnlineStream.Name}
        };
        OnlineStream = null;
        await Shell.Current.GoToAsync($"///{nameof(PlayerPage)}", parameters);
    }

    [RelayCommand]
    private async Task SelectedEpgAsync()
    {
        if(SelectedEpg is null)
            return;
        var link = ChannelLink;
        var stamp = TimeStamps[int.Parse(SelectedEpg.Id)];
        link = link.Replace("%TIMESTAMP%", stamp);
        
        SelectedEpg = null;

        var parameters = new Dictionary<string, object>
        {
            { "channelLink", link },
            { "channelName", ChannelName }
        };
        await Shell.Current.GoToAsync($"///{nameof(PlayerPage)}", parameters);
    }

    partial void OnSelectedEpgChanged(Epg value)
    {
        if (value is not null)
        {
            SelectedEpgCommand.Execute(null);
        }
    }

    private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        var offset = new TimeSpan(3, 0, 0);
        var newTime = DateTimeOffset.FromUnixTimeSeconds((long)unixTimeStamp).ToOffset(offset);
        return newTime.DateTime;
    }

    private string GetNormalTime(string unixtime)
    {
        var culture = new CultureInfo("ru-RU");
        return UnixTimeStampToDateTime(double.Parse(unixtime)).ToString(culture); 
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("channelId") && query["channelId"] is not null)
            ChannelId = query["channelId"] as string;

        if (query.ContainsKey("channelLink") && query["channelLink"] is not null)
            ChannelLink = query["channelLink"] as string;

        if (query.ContainsKey("channelName") && query["channelName"] is not null)
            ChannelName = query["channelName"] as string;

        LoadEpgsCommand.Execute(null);  
    }
    
    public void ClearBackwardsFromEpg()
    {
        TimeStamps.Clear();
        Epgs.Clear();
        ChannelId = null;
        ChannelLink = null;
        ChannelName = null;
    }

    public void ClearBackwardsFromPlayer()
    {
        TimeStamps.Clear();
        Epgs.Clear();
    }
}