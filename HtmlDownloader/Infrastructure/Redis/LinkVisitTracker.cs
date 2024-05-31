using Domain.Interfaces;
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

    public async Task<bool> ContainsLink(Uri uri)
    {
        return await _database.SetContainsAsync(CacheKey, uri.OriginalString);
    }

    public async Task TrackLink(Uri uri)
    {
        try
        {
            await _semaphore.WaitAsync();

            await _database.SetAddAsync(CacheKey, uri.OriginalString);
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
