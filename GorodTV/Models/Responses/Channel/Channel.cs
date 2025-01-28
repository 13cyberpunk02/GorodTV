namespace GorodTV.Models.Responses.Channel;

public record Channel
{
    public required string Id {  get; set; }
    public required string Name { get; set; }
    public required string EpgId { get; set; }
    public required string Icon { get; set; }
    public required string IconSvg { get; set; }
    public required string Category {  get; set; }
    public required string Link { get; set; }
}


public record ChannelsList
{
    public required List<Channel> Channels { get; set; }
}