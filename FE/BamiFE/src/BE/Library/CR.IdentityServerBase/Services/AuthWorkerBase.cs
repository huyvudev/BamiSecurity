using CR.Constants.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace CR.IdentityServerBase.Services
{
    public abstract class AuthWorkerBase<TDbContext> : IHostedService, IDisposable
        where TDbContext : DbContext
    {
        protected readonly ILogger _logger;
        protected readonly IServiceProvider _serviceProvider;
        private Timer? _timerPrune;

        protected AuthWorkerBase(ILogger logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public virtual Task StartAsync(CancellationToken cancellationToken)
        {
            _timerPrune = new Timer(
                PruneAsync,
                null,
                TimeSpan.Zero,
                IdentityServerConfigs.PrunedPeriod
            );
            return Task.CompletedTask;
        }

        private void PruneAsync(object? state)
        {
            _logger.LogInformation("{0}: Prune old token", nameof(PruneAsync));
            using var scope = _serviceProvider.CreateScope();
            var tokenManager = scope.ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();
            var openIddictAuthorizationManager =
                scope.ServiceProvider.GetRequiredService<IOpenIddictAuthorizationManager>();
            Task.Run(async () =>
                {
                    await tokenManager.PruneAsync(IdentityServerConfigs.ThresholdPruned);
                    await openIddictAuthorizationManager.PruneAsync(
                        IdentityServerConfigs.ThresholdPruned
                    );
                })
                .Wait();
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            _timerPrune?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timerPrune?.Dispose();
        }
    }
}
