using System.Text.Json.Serialization;

namespace GorodTV.Models.Responses.Epg;

public class Epg
{
    [JsonPropertyName("caption")]
    public required string Caption { get; set; }

    [JsonPropertyName("description")]
    public required string Description { get; set; }

    [JsonPropertyName("start_time")]
    public required string Start_Time { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }
    
    [JsonPropertyName("record")]
    public required bool Record {  get; set; }
}
    
    
public class EpgsList
{
    [JsonPropertyName("epg")]
    public required List<Epg> Epgs { get; set; }
}
  