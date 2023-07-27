using StackExchange.Redis;
using System.Text.Json;
using System.Threading.Tasks;
namespace RedisCachingService;
public class CacheService : ICacheService
{
    private IDatabase _cacheDb;

    public CacheService()
    {
        //var redis = ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false");
        //_cacheDb = redis.GetDatabase();

        try
        {
            var redis = ConnectionMultiplexer.Connect("localhost:6379");
            //var redis = ConnectionMultiplexer.Connect("redis:6379");
            _cacheDb = redis.GetDatabase();
        }
        catch (RedisConnectionException ex)
        {
            // Handle connection exception
            Console.WriteLine("Failed to connect to Redis: " + ex.Message);
            // Perform appropriate error handling, such as logging or retry logic
            throw; // Rethrow the exception to indicate a failure in connecting to Redis
        }
    }
        //public CacheService()
        //{
        //    try
        //    {
        //        var options = ConfigurationOptions.Parse("localhost:6379,abortConnect=false");
        //        var redis = ConnectionMultiplexer.Connect(options);
        //        _cacheDb = redis.GetDatabase();
        //    }
        //    catch (RedisConnectionException ex)
        //    {
        //        // Handle connection exception
        //        Console.WriteLine("Failed to connect to Redis: " + ex.Message);
        //        // Perform appropriate error handling, such as logging or retry logic
        //        throw; // Rethrow the exception to indicate a failure in connecting to Redis
        //    }
        //}

    //    public CacheService()
    //{
    //    try
    //    {
    //        var options = ConfigurationOptions.Parse("localhost:6379,abortConnect=false");
    //        options.ConnectTimeout = 5000; // Set the connection timeout to 5000ms
    //        var redis = ConnectionMultiplexer.Connect(options);
    //        _cacheDb = redis.GetDatabase();
    //        _cacheDb.Ping(); // Test the connection to ensure it is successful
    //    }
    //    catch (RedisConnectionException ex)
    //    {
    //        // Handle connection exception
    //        Console.WriteLine("Failed to connect to Redis: " + ex.Message);
    //        // Perform appropriate error handling, such as logging or retry logic
    //        throw; // Rethrow the exception to indicate a failure in connecting to Redis
    //    }
    //}



    //}

    public T GetData<T>(string key)
    {
        var value=_cacheDb.StringGet(key);
        if (!string.IsNullOrEmpty(value))
            return JsonSerializer.Deserialize<T>(value);

        return default;
    }

    public bool Checkkey(string key)
    {
        
            if (_cacheDb.KeyExists(key))
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
    public async Task<bool> IsCacheExpiredAsync(string Key)
    {
        var ttl = await _cacheDb.KeyTimeToLiveAsync(Key);
        return ttl.HasValue && ttl.Value < TimeSpan.Zero;
    }
}

