using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GorodTV.Models.Responses.Channel;
using GorodTV.Pages;
using GorodTV.Services;
using GorodTV.Services.Interfaces;

namespace GorodTV.ModelViews;

public partial class ChannelViewModel :  ObservableObject, IQueryAttributable
{
    [ObservableProperty] 
    private string _categoryId;
    [ObservableProperty] 
    private string _categoryName;
    [ObservableProperty] 
    private ObservableCollection<Channel> _channels;
    [ObservableProperty]
    private Channel _selectedChannel;

    private IRestService _restService { get; }
    private List<Channel> allChannels;
    public IAsyncRelayCommand LoadChannelsCommand { get; } 
    public ChannelViewModel()
    {
        Channels = new ObservableCollection<Channel>();
        allChannels = new List<Channel>();
        _restService = new RestService();
        LoadChannelsCommand = new AsyncRelayCommand(LoadChannelsAsync);
    }
    
    private async Task LoadChannelsAsync()
    {
        if(allChannels.Count > 0)
        {
            FilterChannels();
            return;
        }

        var channelsResponse = await _restService.GetChannelsRequest();
        if (!channelsResponse.Channels.Any())
        {
            await Shell.Current.DisplayAlert("Ошибка", "Похоже что вы не авторизованы", "OK");
            return;
        }

        allChannels = channelsResponse.Channels;
        
        FilterChannels();
    }
    
    private void FilterChannels()
    {
        if (string.IsNullOrEmpty(CategoryId))
            return;

        var filteredChannels = allChannels
            .Where(c => c.Category == CategoryId)
            .ToList();

        Channels.Clear();
        foreach (var channel in filteredChannels)
        {
            Channels.Add(channel);
        }
    }

    [RelayCommand]
    private async Task SelectChannelAsync()
    {
        if(SelectedChannel is null)
            return;
        var link = SelectedChannel.Link;
        link = link.Replace("%IPMAC%", "ssiptv");
        
        var parameters = new Dictionary<string, object>
        {
            { "channelId", SelectedChannel.Id },            
            { "channelLink", link },
            { "channelName", SelectedChannel.Name },
            { "categoryId", CategoryId },
            { "categoryName", CategoryName }
            
        };
        SelectedChannel = null;
        await Shell.Current.GoToAsync($"///{nameof(EpgsPage)}", parameters);
    }

    partial void OnSelectedChannelChanged(Channel value)
    {
        if (value != null)
        {
            SelectChannelCommand.Execute(null);
        }
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.ContainsKey("categoryId") && query["categoryId"] is not null)
            CategoryId = query["categoryId"] as string;

        if (query.ContainsKey("categoryName") && query["categoryName"] is not null)
            CategoryName = query["categoryName"] as string;

        LoadChannelsCommand.Execute(null);
    }
}