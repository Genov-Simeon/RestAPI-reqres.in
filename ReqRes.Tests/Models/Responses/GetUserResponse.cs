using System.Text.Json.Serialization;

namespace Models;

public class GetUserResponse
{
    [JsonPropertyName("data")]
    public User Data { get; set; }

    [JsonPropertyName("support")]
    public Support Support { get; set; }
} 