using Microsoft.Azure.WebJobs.Host.Listeners;
using System.Threading;
using System.Threading.Tasks;

namespace FuncInjector
{
    public class InjectorConfigTriggerListener : IListener
    {
        public void Dispose()
        {

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Cancel()
        {

        }
    }
}
