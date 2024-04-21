using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAPI.HealthChecks;

public class ResponseTimeHealthCheck : IHealthCheck
{
    private Random rnd = new Random();

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        int responseTimeMS = rnd.Next(1, 300);

        if (responseTimeMS < 100) 
        {
            return Task.FromResult(HealthCheckResult.Healthy($"The reponse time looks good ({responseTimeMS})"));
        }
        else if (responseTimeMS < 200)
        {
            return Task.FromResult(HealthCheckResult.Degraded($"The reponse time is a bit slow ({responseTimeMS})"));
        }
        else
        {
            return Task.FromResult(HealthCheckResult.Unhealthy($"The reponse time is unacceptable ({responseTimeMS})"));
        }
    }
}
