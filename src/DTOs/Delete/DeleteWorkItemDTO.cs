using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AzureDevopsTracker.Dtos.Delete
{
    public record DeleteWorkItemDto
    {
        [JsonPropertyName("resource")]
        [JsonProperty("resource")]
        public Resource Resource { get; init; }
    }
}
