using AzureDevopsTracker.Entities;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Interfaces
{
    public interface IChangeLogService
    {
        Task<int> CountItemsForRelease();
        Task<ChangeLog> Release();
        string SendToMessengers(ChangeLog changeLog);
    }
}