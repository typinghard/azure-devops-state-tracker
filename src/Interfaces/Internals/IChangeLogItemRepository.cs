using AzureDevopsTracker.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Interfaces.Internals
{
    public interface IChangeLogItemRepository : IRepository<ChangeLogItem>
    {
        Task<int> CountItemsForRelease();
        Task<IEnumerable<ChangeLogItem>> ListWaitingForRelease();
    }
}