
using Dapr.Client;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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

    await _client.PublishEventAsync("pubsub", "throughput-test-step-1", dto);
  }

  public async Task CountMovesAsync(CountMovesDto dto)
  {
    await _client.PublishEventAsync("pubsub", "count-moves-test", dto);
  }
}
