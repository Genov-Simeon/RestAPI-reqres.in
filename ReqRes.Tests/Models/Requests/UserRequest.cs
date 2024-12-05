using System.Text.Json.Serialization;

namespace Models;

public class UserRequest
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("job")]
    public string Job { get; set; }

    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
} 