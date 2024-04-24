
using Dapr;
using Dapr.Client;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DeepBlue.Api.MoveValidator.Controllers;

[EnableCors(CorsPolicies.AllowGateway)]
public class TestController : ControllerBase
{
  private readonly DaprClient _client;

  public TestController()
  {
    _client = new DaprClientBuilder().Build();
  }

  public async Task ThroughputTest([Topic("make-move-pubsub", "throughput-test-step-1")] ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: MoveValidator.TestController.ThroughputTest"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    await _client.PublishEventAsync("make-move-pubsub", "throughput-test-step-2", dto);
  }
}
