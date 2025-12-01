using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Sample.Api
{
    internal sealed class HealthCheck() : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (MemoryIssueTracker.HadOutOfMemory)
                {
                    return await Task.FromResult(HealthCheckResult.Unhealthy("OutOfMemoryException was detected earlier."));
                }

                return await Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new HealthCheckResult(status: context.Registration.FailureStatus, exception: ex));
            }
        }
    }
}
