using AzureDevopsTracker.Entities;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Interfaces.Internals
{
    public interface IChangeLogRepository : IRepository<ChangeLog>
    {
        Task<int> CountChangeLogsCreatedToday();
    }
}