using Infomatrix.Core.Application.Abstractions.Services;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infomatrix.Core.Infrastructure.Cache;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly TimeSpan DefaultExpirationTime = TimeSpan.FromMinutes(60);

    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var json = await _cache.GetStringAsync(key, cancellationToken);

        if (string.IsNullOrEmpty(json))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiry = null,
        CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(value, _jsonOptions);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiry
                ?? DefaultExpirationTime
        };

        await _cache.SetStringAsync(key, json, cancellationToken);
    }

    public async Task RemoveAsync(
        string key, 
        CancellationToken cancellationToken = default)
    {
        await _cache.RemoveAsync(key, cancellationToken);
    }
}
