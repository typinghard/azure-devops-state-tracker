using AzureDevopsTracker.Entities;
using System.Threading.Tasks;

namespace AzureDevopsTracker.Integrations
{
    internal class FakeIntegration : MessageIntegration
    {
        internal override async Task Send(ChangeLog changeLog) { }
    }
}
