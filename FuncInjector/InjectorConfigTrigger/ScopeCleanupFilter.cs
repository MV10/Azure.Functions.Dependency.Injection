using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FuncInjector
{
    public class ScopeCleanupFilter : IFunctionInvocationFilter, IFunctionExceptionFilter
    {
        private readonly InjectorConfigTrigger config;

        public ScopeCleanupFilter(InjectorConfigTrigger injectConfiguration) => config = injectConfiguration;

        public Task OnExceptionAsync(FunctionExceptionContext exceptionContext, CancellationToken cancellationToken)
        {
            RemoveScope(exceptionContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutedAsync(FunctionExecutedContext executedContext, CancellationToken cancellationToken)
        {
            RemoveScope(executedContext.FunctionInstanceId);
            return Task.CompletedTask;
        }

        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken) => Task.CompletedTask;

        private void RemoveScope(Guid id)
        {
            if (config.Scopes.TryRemove(id, out var scope))
                scope.Dispose();
        }
    }
}
