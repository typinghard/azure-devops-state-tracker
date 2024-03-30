using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace AzureDevopsTracker.Dtos.Create
{
    public record CreateWorkItemDto
    {
        [JsonPropertyName("resource")]
        [JsonProperty("resource")]
        public Resource Resource { get; init; }
    }
}