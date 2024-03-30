using AzureDevopsTracker.Dtos;
using AzureDevopsTracker.Dtos.Create;
using AzureDevopsTracker.Dtos.Delete;
using AzureDevopsTracker.Dtos.Restore;
using AzureDevopsTracker.Dtos.Update;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Interfaces
{
    public interface IAzureDevopsTrackerService
    {
        Task Create(CreateWorkItemDto createDto, bool addWorkItemChange = true);
        Task Update(UpdatedWorkItemDto updateDto);
        Task Delete(DeleteWorkItemDto deleteDto);
        Task Restore(RestoreWorkItemDto restoreDto);
        Task<WorkItemDto> GetByWorkItemId(string workItemId);
    }
}