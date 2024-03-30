using Newtonsoft.Json;
using System;
using System.Text.Json.Serialization;

namespace AzureDevopsTracker.Dtos
{
    public record Fields
    {
        [JsonPropertyName("System.AreaPath")]
        [JsonProperty("System.AreaPath")]
        public string AreaPath { get; init; }

        [JsonPropertyName("System.TeamProject")]
        [JsonProperty("System.TeamProject")]
        public string TeamProject { get; init; }

        [JsonPropertyName("System.IterationPath")]
        [JsonProperty("System.IterationPath")]
        public string IterationPath { get; init; }

        [JsonPropertyName("System.AssignedTo")]
        [JsonProperty("System.AssignedTo")]
        public string AssignedTo { get; init; }

        [JsonPropertyName("System.WorkItemType")]
        [JsonProperty("System.WorkItemType")]
        public string Type { get; init; }

        [JsonPropertyName("System.CreatedDate")]
        [JsonProperty("System.CreatedDate")]
        public DateTime CreatedDate { get; init; }

        [JsonPropertyName("System.CreatedBy")]
        [JsonProperty("System.CreatedBy")]
        public string CreatedBy { get; init; }

        [JsonPropertyName("System.ChangedBy")]
        [JsonProperty("System.ChangedBy")]
        public string ChangedBy { get; init; }

        [JsonPropertyName("System.State")]
        [JsonProperty("System.State")]
        public string State { get; init; }

        [JsonPropertyName("System.Title")]
        [JsonProperty("System.Title")]
        public string Title { get; init; }

        [JsonPropertyName("System.Tags")]
        [JsonProperty("System.Tags")]
        public string Tags { get; init; }

        [JsonPropertyName("System.Parent")]
        [JsonProperty("System.Parent")]
        public string Parent { get; init; }

        [JsonPropertyName("Microsoft.VSTS.Scheduling.StoryPoints")]
        [JsonProperty("Microsoft.VSTS.Scheduling.StoryPoints")]
        public string StoryPoints { get; init; }

        [JsonPropertyName("Microsoft.VSTS.Scheduling.OriginalEstimate")]
        [JsonProperty("Microsoft.VSTS.Scheduling.OriginalEstimate")]
        public string OriginalEstimate { get; init; }

        [JsonPropertyName("Microsoft.VSTS.Scheduling.RemainingWork")]
        [JsonProperty("Microsoft.VSTS.Scheduling.RemainingWork")]
        public string RemainingWork { get; init; }

        [JsonPropertyName("Microsoft.VSTS.Scheduling.Effort")]
        [JsonProperty("Microsoft.VSTS.Scheduling.Effort")]
        public string Effort { get; init; }

        [JsonPropertyName("Microsoft.VSTS.Common.Activity")]
        [JsonProperty("Microsoft.VSTS.Common.Activity")]
        public string Activity { get; init; }

        [JsonProperty("Custom.ChangeLogDescription")]
        public string ChangeLogDescription { get; init; }
    }
}