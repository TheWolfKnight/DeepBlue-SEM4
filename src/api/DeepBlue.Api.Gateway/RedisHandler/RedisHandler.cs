
using DeepBlue.Api.RedisHandler.Interfaces;
using StackExchange.Redis;

namespace DeepBlue.Api.RedisHandler;

public class RedisHandler : IRedisHandler
{
  private readonly ConnectionMultiplexer _connMultiplex;
  private readonly IDatabase _database;

  public RedisHandler()
  {
    _connMultiplex = ConnectionMultiplexer.Connect("localhost");
    _database = _connMultiplex.GetDatabase();
  }

  public async Task<string?> StringGetAsync(string key)
  {
    return await _database.StringGetAsync(key);
  }

  public async Task StringSetAsync(string key, string value)
  {
    await _database.StringSetAsync(key, value);
  }

  public void Dispose()
  {
    _connMultiplex.Close();
  }
}
