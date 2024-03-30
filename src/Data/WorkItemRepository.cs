using AzureDevopsTracker.Data.Context;
using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Interfaces.Internals;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Data
{
    internal class WorkItemRepository : Repository<WorkItem>, IWorkItemRepository
    {
        public WorkItemRepository(AzureDevopsTrackerContext context) : base(context) { }

        public async Task<WorkItem> GetByWorkItemId(string workItemId)
        {
            return await DbSet
                          .Include(x => x.WorkItemsChanges)
                          .Include(x => x.TimeByStates)
                          .Include(x => x.ChangeLogItem)
                          .Include(x => x.CustomFields)
                          .FirstOrDefaultAsync(x => x.Id == workItemId);
        }

        public async Task<IEnumerable<WorkItem>> ListByWorkItemId(IEnumerable<string> workItemsId)
        {
            return await DbSet
                          .Include(x => x.WorkItemsChanges)
                          .Include(x => x.TimeByStates)
                          .Include(x => x.ChangeLogItem)
                          .Include(x => x.CustomFields)
                          .Where(x => workItemsId.Contains(x.Id))
                          .ToListAsync();
        }

        public async Task<IEnumerable<WorkItem>> ListByIterationPath(string iterationPath)
        {
            return await DbSet
                          .Include(x => x.WorkItemsChanges)
                          .Include(x => x.TimeByStates)
                          .Include(x => x.ChangeLogItem)
                          .Include(x => x.CustomFields)
                          .Where(x => x.IterationPath == iterationPath)
                          .ToListAsync();
        }

        public void RemoveAllTimeByState(List<TimeByState> timeByStates)
        {
            if (!timeByStates.Any())
                return;

            Db.TimeByStates.RemoveRange(timeByStates);
        }
    }
}