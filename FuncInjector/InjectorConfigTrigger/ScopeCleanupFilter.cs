using Microsoft.Azure.WebJobs.Host;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace FuncInjector
{
    public class ScopeCleanupFilter : IFunctionInvocationFilter, IFunctionExceptionFilter
    {
        private readonly InjectorConfigTrigger _injectConfiguration;

        public ScopeCleanupFilter(InjectorConfigTrigger injectConfiguration)
        {
            _injectConfiguration = injectConfiguration;
        }


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

        public Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken) =>
            Task.CompletedTask;

        private void RemoveScope(Guid id)
        {
            if (_injectConfiguration.Scopes.TryRemove(id, out var scope))
            {
                scope.Dispose();
            }
        }
    }
}
