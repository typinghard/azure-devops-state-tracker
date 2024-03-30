using AzureDevopsTracker.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Integrations
{
    internal class MessageFacade
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MessageFacade(
            IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task Send(ChangeLog changeLog)
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var messageIntegration = scope.ServiceProvider.GetService<MessageIntegration>();
            if (messageIntegration is null) throw new Exception("Configure the MessageConfig in Startup to send changelog messages");

            await messageIntegration.Send(changeLog);
        }
    }
}
