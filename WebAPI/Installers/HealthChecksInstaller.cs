
using Infrastructure.Data;
using WebAPI.HealthChecks;

namespace WebAPI.Installers;

public class HealthChecksInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddDbContextCheck<BloggerContext>("Database")
            .AddCheck<ResponseTimeHealthCheck>("Network speed test");

        services.AddHealthChecksUI()
            .AddInMemoryStorage();       
    }
}
