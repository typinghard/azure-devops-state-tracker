using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AzureDevopsTracker.Dtos
{
    public record Resource
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; init; }

        [JsonPropertyName("fields")]
        [JsonProperty("fields")]
        public Fields Fields { get; init; }
    }
}
