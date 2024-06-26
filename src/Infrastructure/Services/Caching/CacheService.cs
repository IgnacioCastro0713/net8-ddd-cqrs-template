﻿using Application.Abstractions.Services.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Infrastructure.Services.Caching;

public sealed class CacheService(IDistributedCache cache) : ICacheService
{
    private static readonly TimeSpan DefaultExpiration = TimeSpan.FromMinutes(5);

    private static readonly JsonSerializerSettings JsonSerializerSettings = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public async Task<T> GetOrCreateAsync<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        var data = await GetAsync<T>(key, cancellationToken);

        if (data is null)
        {
            data = await factory(cancellationToken);

            await SetAsync(key, data, expiration, cancellationToken);
        }

        return data;
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var cacheData = await cache.GetStringAsync(key, cancellationToken);

        return cacheData is null
            ? default
            : JsonConvert.DeserializeObject<T>(cacheData);
    }

    public async Task SetAsync<T>(
        string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken cancellationToken = default)
    {
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration ?? DefaultExpiration
        };

        var serializedValue = JsonConvert.SerializeObject(value, JsonSerializerSettings);

        await cache.SetStringAsync(key, serializedValue, cacheOptions, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await cache.RemoveAsync(key, cancellationToken);
    }
    
    public async Task RemoveByKeysAsync(IEnumerable<string> keys, CancellationToken cancellationToken = default)
    {
        var tasksToInvalidate = keys.Select(key => cache.RemoveAsync(key, cancellationToken));

        await Task.WhenAll(tasksToInvalidate);
    }
}