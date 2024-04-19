using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICosmosPostRepository, CosmosPostRepository>();
        services.AddScoped<IPictureRepository, PictureRepository>();
        services.AddScoped<IAttachmentRepository, AttachmentRepository>();

        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<BloggerContext>()
            .AddDefaultTokenProviders();

        return services;
    }
}
