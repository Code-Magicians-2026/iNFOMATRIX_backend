using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Infrastructure.AI.Options;
using Infomatrix.Core.Infrastructure.AI.Services;
using Infomatrix.Core.Infrastructure.Auth;
using Infomatrix.Core.Infrastructure.Cache;
using Infomatrix.Core.Infrastructure.Persistence;
using Infomatrix.Core.Infrastructure.Persistence.Options;
using Infomatrix.Core.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Refit;
using SupabaseClient = Supabase.Client;
using SupabaseSDKOptions = Supabase.SupabaseOptions;

namespace Infomatrix.Core.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSupabase(configuration);
        services.AddDatabase(configuration);
        services.AddAiMicroservice(configuration);

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddDistributedMemoryCache();
        services.AddScoped<AiMicroserviceService>();
        services.AddScoped<IAIService>(sp => sp.GetRequiredService<AiMicroserviceService>());
        services.AddScoped<IVisionService>(sp => sp.GetRequiredService<AiMicroserviceService>());

        services.AddOptions<SupabaseOptions>()
            .Bind(configuration.GetSection(SupabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<AiServiceOptions>()
            .Bind(configuration.GetSection(AiServiceOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(configuration["Database:ConnectionString"],
            providerOptions =>
            {
                providerOptions.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(2),
                    null);

                providerOptions.CommandTimeout(15);
            });
            options.EnableSensitiveDataLogging(true);
            options.EnableDetailedErrors(true);
        });
    }

    private static IServiceCollection AddAiMicroservice(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection(AiServiceOptions.SectionName)
            .Get<AiServiceOptions>()
            ?? throw new InvalidOperationException("AI service settings are missing.");

        services.AddRefitClient<IAiMicroserviceApi>()
            .ConfigureHttpClient(client =>
            {
                client.BaseAddress = new Uri(options.BaseAddress);
            });

        return services;
    }

    private static IServiceCollection AddSupabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddSingleton(sp =>
        {
            var supabaseOptions = sp.GetRequiredService<IOptions<SupabaseOptions>>().Value;

            return new SupabaseClient(
                configuration["Supabase:Url"]!,
                configuration["Supabase:Key"],
                new SupabaseSDKOptions
                {
                    AutoRefreshToken = false,
                });
        });
    }
}
