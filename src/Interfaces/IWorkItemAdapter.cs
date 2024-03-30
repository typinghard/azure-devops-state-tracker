using AzureDevopsTracker.Dtos;
using AzureDevopsTracker.Entities;
using System.Collections.Generic;

namespace AzureDevopsTracker.Interfaces
{
    public interface IWorkItemAdapter
    {
        WorkItemDto ToWorkItemDto(WorkItem workItem);
        List<WorkItemDto> ToWorkItemsDto(List<WorkItem> workItems);
    }
}