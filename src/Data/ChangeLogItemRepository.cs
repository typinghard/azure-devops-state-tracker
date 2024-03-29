using AzureDevopsTracker.Data.Context;
using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Interfaces.Internals;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Data
{
    internal class ChangeLogItemRepository : Repository<ChangeLogItem>, IChangeLogItemRepository
    {
        public ChangeLogItemRepository(AzureDevopsTrackerContext context) : base(context) { }

        public async Task<int> CountItemsForRelease()
        {
            return await DbSet.CountAsync(x => string.IsNullOrEmpty(x.ChangeLogId));
        }

        public async Task<IEnumerable<ChangeLogItem>> ListWaitingForRelease()
        {
            return await DbSet.Where(x => string.IsNullOrEmpty(x.ChangeLogId)).ToListAsync();
        }
    }
}