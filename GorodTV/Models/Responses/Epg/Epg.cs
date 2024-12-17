using System.Text.Json.Serialization;

namespace GorodTV.Models.Responses.Epg;

public record Epg(
    [property: JsonPropertyName("caption")] string Caption,
    [property: JsonPropertyName("description")] string Description,
    [property: JsonPropertyName("start_time")] string Start_Time,
    [property: JsonPropertyName("id")] string Id,
    [property: JsonPropertyName("record")] bool Record);
    
public record EpgsList(
    [property: JsonPropertyName("epg")] List<Epg> Epgs
);