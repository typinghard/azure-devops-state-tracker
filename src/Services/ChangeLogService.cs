using AzureDevopsTracker.Entities;
using AzureDevopsTracker.Integrations;
using AzureDevopsTracker.Interfaces;
using AzureDevopsTracker.Interfaces.Internals;
using AzureDevopsTracker.Statics;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Services
{
    public class ChangeLogService : IChangeLogService
    {
        private readonly IChangeLogItemRepository _changeLogItemRepository;
        private readonly IChangeLogRepository _changeLogRepository;
        private readonly IConfiguration _configuration;
        private readonly MessageIntegration _messageIntegration;

        public ChangeLogService(
            IChangeLogItemRepository changeLogItemRepository,
            IChangeLogRepository changeLogRepository,
            IConfiguration configuration,
            MessageIntegration messageIntegration)
        {
            _changeLogItemRepository = changeLogItemRepository;
            _changeLogRepository = changeLogRepository;
            _configuration = configuration;
            _messageIntegration = messageIntegration;
        }

        public async Task<int> CountItemsForRelease()
        {
            return await _changeLogItemRepository.CountItemsForRelease();
        }

        public async Task<ChangeLog> Release()
        {
            var changeLogItems = await _changeLogItemRepository.ListWaitingForRelease();
            if (!changeLogItems.Any()) return null;

            var changeLog = await CreateChangeLog();
            changeLog.AddChangeLogItems(changeLogItems);

            await _changeLogRepository.Add(changeLog);
            await _changeLogRepository.SaveChangesAsync();

            return changeLog;
        }

        public async Task<string> SendToMessengers(ChangeLog changeLog)
        {
            await _messageIntegration.Send(changeLog);

            return $"The ChangeLog {changeLog.Number} was released.";
        }

        private async Task<ChangeLog> CreateChangeLog()
        {
            if (string.IsNullOrEmpty(_configuration[ConfigurationStatics.ADT_CHANGELOG_VERSION]))
            {
                var changeLogsQuantity = await _changeLogRepository.CountChangeLogsCreatedToday();
                return new ChangeLog(changeLogsQuantity + 1);
            }
            return new ChangeLog(_configuration[ConfigurationStatics.ADT_CHANGELOG_VERSION]);
        }
    }
}