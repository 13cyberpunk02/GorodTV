namespace GorodTV.Models.Responses.Channel;

public record Channel( 
    string Id,
    string Name,
    string EpgId,
    string Icon,
    string IconSvg,
    string Category,
    string Link);

public record ChannelsList(List<Channel> Channels);