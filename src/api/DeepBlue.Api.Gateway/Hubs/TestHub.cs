
using Dapr.Client;
using Dapr;
using DeepBlue.Api.RedisHandler.Interfaces;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace DeepBlue.Api.Gateway.Hubs;

[EnableCors]
public class TestHub : Hub
{

  private readonly DaprClient _client;
  private readonly IRedisHandler _redis;

  public TestHub(IRedisHandler redis)
  {
    _client = new DaprClientBuilder().Build();
    _redis = redis;
  }

  public async Task TestEchoAsync(string message)
  {
    await Task.Run(() => Console.WriteLine(message));
  }

  public async Task TestThroughputAsync(ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: Gateway.TestHub.TestThroughputAsync"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    await _redis.StringSetAsync($"test-{dto.ConnectionId}", $"{dto.ConnectionId}");

    await _client.PublishEventAsync("make-move-pubsub", "throughput-test-step-1", dto);
  }

  [Topic("make-move-pubsub", "throughput-test-step-3")]
  public async Task TestThroughputEndAsync(ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: Gateway.TestHub.TestThroughputEndAsync"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    string connection = await _redis.StringGetAsync($"test-{dto.ConnectionId}")
      ?? throw new Exception("Could not find the connection in redis");

    await Clients.All.SendAsync("TestThroughputEndAsync", dto);
  }
}
