using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FuncInjector
{
    /// <summary>
    /// Tear-down of any scoped-lifetime services.
    /// </summary>
    public class ReleaseScopedServicesFilter : IFunctionInvocationFilter, IFunctionExceptionFilter
    {
        private readonly RegisterServicesTrigger trigger;

        public ReleaseScopedServicesFilter(RegisterServicesTrigger regSvcTrigger) => trigger = regSvcTrigger;

        public Task OnExceptionAsync(FunctionExceptionContext context, CancellationToken cancellationToken)
        {
            RemoveScope(context.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutedAsync(FunctionExecutedContext context, CancellationToken cancellationToken)
        {
            RemoveScope(context.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutingAsync(FunctionExecutingContext context, CancellationToken cancellationToken) => Task.CompletedTask;

        private void RemoveScope(Guid id)
        {
            if (trigger.Scopes.TryRemove(id, out var scope)) scope.Dispose();
        }
    }
}
