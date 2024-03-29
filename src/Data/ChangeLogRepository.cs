using AzureDevopsTracker.Data.Context;
using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Interfaces.Internals;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Data
{
    internal class ChangeLogRepository : Repository<ChangeLog>, IChangeLogRepository
    {
        public ChangeLogRepository(AzureDevopsTrackerContext context) : base(context) { }

        public async Task<int> CountChangeLogsCreatedToday()
        {
            return await DbSet.CountAsync(x => x.CreatedAt.Date == DateTime.Now.Date);
        }
    }
}