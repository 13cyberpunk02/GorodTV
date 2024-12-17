using GorodTV.Models.Responses.Category;
using GorodTV.Models.Responses.Channel;
using GorodTV.Models.Responses.Epg;
using GorodTV.Models.Responses.UnixTime;

namespace GorodTV.Services.Interfaces;

public interface IRestService
{
    Task<CategoriesList> GetCategoriesRequest();
    Task<ChannelsList> GetChannelsRequest();
    Task<EpgsList> GetEpgOneDay(string startTime, string channelId);
    Task<UnixTime> GetUnixTimeAsync();
}