using Microsoft.Azure.WebJobs.Host.Listeners;
using System.Threading;
using System.Threading.Tasks;

namespace FuncInjector
{
    /// <summary>
    /// RegisterServices trigger only needs to provide basic start/stop acknowledgements.
    /// </summary>
    public class RegisterServicesTriggerListener : IListener
    {
        public void Dispose() { }

        public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public void Cancel() { }
    }
}
