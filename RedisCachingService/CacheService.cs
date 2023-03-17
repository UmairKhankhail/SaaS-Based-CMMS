using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;
namespace RedisCachingService;
public class CacheService : ICacheService
{
    private IDatabase _cacheDb;

    public CacheService()
    {
        var redis = ConnectionMultiplexer.Connect("localhost:6379");
        _cacheDb = redis.GetDatabase();
    }

    public T GetData<T>(string key)
    {
        var value=_cacheDb.StringGet(key);
        if (!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<T>(value);

        return default;
    }

    public bool Checkkey(string key)
    {
        if(_cacheDb.KeyExists(key))
        {
            return true;
        }
        return false;
    }

    public object RemoveData(string key)
    {
        var exists = _cacheDb.KeyExists(key);
        if(exists)
            return _cacheDb.KeyDelete(key);
        return false;
    }

    public bool SetData<T>(string key, T value, DateTimeOffset expirationTime)
    {
        var expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
        return _cacheDb.StringSet(key, JsonSerializer.Serialize(value), expiryTime);
    }
    public async Task<bool> UpdateDataAsync<T>(string key, T value)
    {
        var expiryTime = await _cacheDb.KeyTimeToLiveAsync(key);
        return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expiryTime);
    }
}

