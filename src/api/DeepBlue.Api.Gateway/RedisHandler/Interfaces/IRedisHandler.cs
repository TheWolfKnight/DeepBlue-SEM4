
namespace DeepBlue.Api.RedisHandler.Interfaces;

public interface IRedisHandler : IDisposable
{
  Task StringSetAsync(string key, string value);
  Task<string?> StringGetAsync(string key);
}
