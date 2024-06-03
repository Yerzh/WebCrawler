using Domain.Interfaces;
using Domain.ValueObjects;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Infrastructure.Redis;

public class LinkVisitTracker : ILinkVisitTracker, IDisposable
{
    const string CacheKey = "MySet";

    private readonly ConnectionMultiplexer _redis;
    private readonly IDatabase _database;

    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    public LinkVisitTracker(IOptions<RedisConfig> options)
    {
        var config = options.Value;
        _redis = ConnectionMultiplexer.Connect(config.ConnectionString);
        _database = _redis.GetDatabase();
    }

    public async Task<bool> ContainsLink(Link link)
    {
        return await _database.SetContainsAsync(CacheKey, link.UriString);
    }

    public async Task TrackLink(Link link)
    {
        try
        {
            await _semaphore.WaitAsync();

            await _database.SetAddAsync(CacheKey, link.UriString);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public void Dispose()
    {
        _redis.Close();

        GC.SuppressFinalize(this);
    }
}
