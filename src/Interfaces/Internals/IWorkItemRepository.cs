using AzureDevopsTracker.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Interfaces.Internals
{
    public interface IWorkItemRepository : IRepository<WorkItem>
    {
        Task<WorkItem> GetByWorkItemId(string workItemId);
        Task<IEnumerable<WorkItem>> ListByWorkItemId(IEnumerable<string> workItemsId);
        Task<IEnumerable<WorkItem>> ListByIterationPath(string iterationPath);
        void RemoveAllTimeByState(List<TimeByState> timeByStates);
    }
}