using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace AzureDevopsTracker.Dtos.Update
{
    public record UpdatedWorkItemDto
    {
        [JsonPropertyName("resource")]
        [JsonProperty("resource")]
        public Resource Resource { get; init; }
    }

    public record Resource
    {
        [JsonPropertyName("workItemId")]
        [JsonProperty("workItemId")]
        public string WorkItemId { get; init; }

        [JsonPropertyName("fields")]
        [JsonProperty("fields")]
        public ResourceFields Fields { get; init; }

        [JsonPropertyName("revision")]
        [JsonProperty("revision")]
        public Revision Revision { get; init; }
    }

    public record Revision
    {
        [JsonPropertyName("fields")]
        [JsonProperty("fields")]
        public Fields Fields { get; init; }
    }

    public record ResourceFields
    {
        [JsonPropertyName("System.State")]
        [JsonProperty("System.State")]
        public OldNewValues State { get; init; }

        [JsonPropertyName("Microsoft.VSTS.Common.StateChangeDate")]
        [JsonProperty("Microsoft.VSTS.Common.StateChangeDate")]
        public DateTimeOldNewValues StateChangeDate { get; init; }

        [JsonPropertyName("System.ChangedBy")]
        [JsonProperty("System.ChangedBy")]
        public OldNewValues ChangedBy { get; init; }
    }

    public record OldNewValues
    {
        [JsonPropertyName("oldValue")]
        [JsonProperty("oldValue")]
        public string OldValue { get; init; }

        [JsonPropertyName("newValue")]
        [JsonProperty("newValue")]
        public string NewValue { get; init; }
    }

    public record DateTimeOldNewValues
    {
        [JsonPropertyName("oldValue")]
        [JsonProperty("oldValue")]
        public DateTime OldValue { get; init; }

        [JsonPropertyName("newValue")]
        [JsonProperty("newValue")]
        public DateTime NewValue { get; init; }
    }

}