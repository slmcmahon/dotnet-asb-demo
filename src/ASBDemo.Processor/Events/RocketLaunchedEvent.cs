using System.Text.Json.Serialization;

namespace ASBDemo.Processor.Events;

public class RocketLaunchedEvent
{
    [JsonPropertyName("rocketName")]
    public string? RocketName { get; set; }

    [JsonPropertyName("launchDateTime")]
    public DateTime? LaunchDateTime { get; set; }

    [JsonPropertyName("launchLocation")]
    public string? LaunchLocation { get; set; }

    [JsonPropertyName("payload")]
    public string? Payload { get; set; }

    [JsonPropertyName("destination")]
    public string? Destination { get; set; }

    [JsonPropertyName("mission")]
    public string? Mission { get; set; }

    [JsonPropertyName("status")]
    public string? Status { get; set; }
}