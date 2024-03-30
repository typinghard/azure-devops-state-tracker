using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AzureDevopsTracker.Dtos.Restore
{
    public record RestoreWorkItemDto
    {
        [JsonPropertyName("resource")]
        [JsonProperty("resource")]
        public Resource Resource { get; init; }
    }
}
