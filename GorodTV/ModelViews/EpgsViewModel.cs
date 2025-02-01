using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GorodTV.Models.Responses.Epg;
using GorodTV.Models.Responses.OnlineStream;
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
    private OnlineStream? _onlineStream;
    
    [ObservableProperty] 
    private Dictionary<int, string> _timeStamps;
    
    [ObservableProperty] 
    private ObservableCollection<List<Epg>> _epgs;

    [ObservableProperty]
    private Epg _selectedEpg;
    
    private readonly IRestService _restService;
    public IAsyncRelayCommand LoadEpgsCommand { get; }
    public IAsyncRelayCommand StartLiveBroadcastCommand { get; }

    public EpgsViewModel()
    {
        Epgs = new();
        TimeStamps = new();        
        _restService = new RestService();
        LoadEpgsCommand = new AsyncRelayCommand(LoadEpgsAsync);
        StartLiveBroadcastCommand = new AsyncRelayCommand(StartLiveBroadcastAsync);
    }


    private async Task LoadEpgsAsync()
    {
        if (Epgs.Any())
            return;

        var unixTime = await _restService.GetUnixTimeAsync();        
        var epgsFor2Weeks = await _restService.GetEpgsForTwoWeeks(unixTime.Unixtime, ChannelId);
        
        if (epgsFor2Weeks.Any())
        {
            OnlineStream = new OnlineStream
            {
                Description = "Прямой эфир",
                Link = ChannelLink,
                Name = ChannelName
            };

            foreach (var epgs in epgsFor2Weeks)
            {
                var sortedEpg = epgs.Epgs.OrderBy(e => UnixTimeStampToDateTime(double.Parse(e.Start_Time))).ToList();
                var epgsToAdd = new List<Epg>();
                foreach (var epg in sortedEpg)
                {
                    if (!TimeStamps.ContainsKey(int.Parse(epg.Id)))
                        TimeStamps.Add(int.Parse(epg.Id), epg.Start_Time);

                    if (DateTime.Now < UnixTimeStampToDateTime(double.Parse(epg.Start_Time)))
                        break;

                    epgsToAdd.Add(new Epg
                    {
                        Caption = epg.Caption,
                        Description = epg.Description,
                        Id = epg.Id,
                        Record = epg.Record,
                        Start_Time = GetNormalTime(epg.Start_Time)
                    });
                }
                Epgs.Add(epgsToAdd);                
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
            { "channelLink", link }
        };          
        await Shell.Current.GoToAsync("category/channel/epg/player", parameters);
    }

    [RelayCommand]
    private async Task SelectedEpgAsync(Epg epg)
    {
        if(epg is null)
            return;
        SelectedEpg = epg;
        var link = ChannelLink;
        var stamp = TimeStamps[int.Parse(SelectedEpg.Id)];
        link = link.Replace("%TIMESTAMP%", stamp);

        SelectedEpg = null;

        var parameters = new Dictionary<string, object>
        {
            { "channelLink", link }
        };
        await Shell.Current.GoToAsync("category/channel/epg/player", parameters);
    }

    partial void OnSelectedEpgChanged(Epg? value)
    {
        if (value is not null)
        {
            SelectedEpgCommand.Execute(value);
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
}