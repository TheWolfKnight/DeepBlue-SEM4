
using Dapr.Client;
using Dapr;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace DeepBlue.Api.Gateway.Hubs;

[EnableCors]
public class TestHub : Hub
{

  private readonly DaprClient _client;

  public TestHub()
  {
    _client = new DaprClientBuilder().Build();
  }

  public async Task TestEchoAsync(string message)
  {
    await Task.Run(() => Console.WriteLine(message));
  }

  public async Task TestThroughputAsync(ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: Gateway.TestHub.TestThroughputAsync"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    await _client.PublishEventAsync("make-move-pubsub", "throughput-test-step-1", dto);
  }

  [Topic("make-move-pubsub", "throughput-test-step-3")]
  public async Task TestThroughputEndAsync(ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: Gateway.TestHub.TestThroughputEndAsync"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    await Clients.All.SendAsync("TestThroughputEndAsync", dto);
  }
}
