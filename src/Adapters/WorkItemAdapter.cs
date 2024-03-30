using AzureDevopsTracker.Dtos;
using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Extensions;
using AzureDevopsTracker.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzureDevopsTracker.Adapters
{
    internal class WorkItemAdapter : IWorkItemAdapter
    {
        public WorkItemDto ToWorkItemDto(WorkItem workItem)
        {
            if (workItem is null) return null;

            return new WorkItemDto()
            {
                Id = workItem.Id,
                CreatedAt = workItem.CreatedAt,
                AssignedTo = workItem.AssignedTo,
                CreatedBy = workItem.CreatedBy,
                CurrentStatus = workItem.CurrentStatus,
                TeamProject = workItem.TeamProject,
                AreaPath = workItem.AreaPath,
                IterationPath = workItem.IterationPath,
                Title = workItem.Title,
                Type = workItem.Type,
                Effort = workItem.Effort,
                StoryPoints = workItem.StoryPoints,
                OriginalEstimate = workItem.OriginalEstimate,
                WorkItemParentId = workItem.WorkItemParentId,
                Activity = workItem.Activity,
                Tags = workItem.Tags is null ? new List<string>() : workItem.Tags.Split(';').ToList(),
                WorkItemsChangesDto = ToWorkItemsChangeDto(workItem.WorkItemsChanges.OrderBy(x => x.CreatedAt).ToList()),
                TimesByStateDto = ToTimeByStatesDto(workItem.CalculateTotalTimeByState().ToList()),
            };
        }

        public List<WorkItemDto> ToWorkItemsDto(List<WorkItem> workItems)
        {
            var workItemsDto = new List<WorkItemDto>();

            if (workItems is null) return workItemsDto;

            workItems.ForEach(
                        workItem =>
                        workItemsDto.Add(ToWorkItemDto(workItem)));

            return workItemsDto
                     .ToList();
        }

        public static WorkItemChangeDto ToWorkItemChangeDto(WorkItemChange workIteChange)
        {
            if (workIteChange is null) return null;

            return new WorkItemChangeDto()
            {
                NewDate = workIteChange.NewDate,
                NewState = workIteChange.NewState,
                OldState = workIteChange.OldState,
                OldDate = workIteChange.OldDate,
                ChangedBy = workIteChange.ChangedBy
            };
        }

        public static List<WorkItemChangeDto> ToWorkItemsChangeDto(List<WorkItemChange> workItemsChanges)
        {
            var workItemsChangeDto = new List<WorkItemChangeDto>();

            if (workItemsChanges is null) return workItemsChangeDto;

            workItemsChanges.ForEach(
                        workItemsChange =>
                        workItemsChangeDto.Add(ToWorkItemChangeDto(workItemsChange)));

            return workItemsChangeDto
                     .Where(w => w is not null)
                     .ToList();
        }

        public static TimeByStateDto ToTimeByStateDto(TimeByState workItemStatusTime)
        {
            if (workItemStatusTime is null) return null;

            return new TimeByStateDto()
            {
                CreatedAt = workItemStatusTime.CreatedAt,
                State = workItemStatusTime.State,
                TotalTime = TimeSpan.FromSeconds(workItemStatusTime.TotalTime).ToTextTime(),
                TotalWorkedTime = TimeSpan.FromSeconds(workItemStatusTime.TotalWorkedTime).ToTextTime()
            };
        }

        public static List<TimeByStateDto> ToTimeByStatesDto(List<TimeByState> workItemStatusTimes)
        {
            var workItemStatusTimeDto = new List<TimeByStateDto>();

            if (workItemStatusTimes is null) return workItemStatusTimeDto;

            workItemStatusTimes.ForEach(
                        workItemStatusTime =>
                        workItemStatusTimeDto.Add(ToTimeByStateDto(workItemStatusTime)));

            return workItemStatusTimeDto
                     .Where(w => w is not null)
                     .ToList();
        }
    }
}