using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Infrastructure.Auth;
using Infomatrix.Core.Infrastructure.Persistence;
using Infomatrix.Core.Infrastructure.Persistence.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infomatrix.Core.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAuthService, AuthService>();

        services.AddOptions<SupabaseOptions>()
            .Bind(configuration.GetSection(SupabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddSingleton(sp =>
        {
            var supabaseOptions = sp.GetRequiredService<IOptions<SupabaseOptions>>().Value;

            return new Supabase.Client(
                supabaseOptions.Url,
                supabaseOptions.Key,
                new Supabase.SupabaseOptions
                {
                    AutoRefreshToken = false,
                });
        });

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var databaseOptions = sp.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            options.UseNpgsql(databaseOptions.ConnectionString);
        });

        return services;
    }
}
