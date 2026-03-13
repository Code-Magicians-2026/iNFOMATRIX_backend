using Infomatrix.Core.Application.Abstractions.Repositories;
using Infomatrix.Core.Application.Abstractions.Services;
using Infomatrix.Core.Infrastructure.AI;
using Infomatrix.Core.Infrastructure.AI.Options;
using Infomatrix.Core.Infrastructure.Auth;
using Infomatrix.Core.Infrastructure.Cache;
using Infomatrix.Core.Infrastructure.Persistence;
using Infomatrix.Core.Infrastructure.Persistence.Options;
using Infomatrix.Core.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
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
        services.AddAI(configuration);

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddDistributedMemoryCache();
        services.AddScoped<IAIService, AIService>();
        services.AddScoped<IVisionService, VisionService>();

        services.AddOptions<SupabaseOptions>()
            .Bind(configuration.GetSection(SupabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<DatabaseOptions>()
            .Bind(configuration.GetSection(DatabaseOptions.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.AddOptions<AIOptions>()
            .Bind(configuration.GetSection(AIOptions.SectionName))
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
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorCodesToAdd: ["23505"]);
                    providerOptions.CommandTimeout(60);
                });

            options.EnableSensitiveDataLogging(false);
            options.EnableDetailedErrors(false);
        });
    }

    private static IServiceCollection AddAI(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var options = configuration
            .GetSection(AIOptions.SectionName)
            .Get<AIOptions>();

        var builder = services.AddKernel();

        builder.AddAzureOpenAIChatCompletion(
            deploymentName: options.DeploymentName,
            endpoint: options.Endpoint,
            apiKey: options.ApiKey);

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
