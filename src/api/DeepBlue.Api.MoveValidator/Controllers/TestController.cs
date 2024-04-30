
using Dapr;
using Dapr.Client;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DeepBlue.Api.MoveValidator.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class TestController : ControllerBase
{
  private readonly DaprClient _client;

  public TestController()
  {
    _client = new DaprClientBuilder().Build();
  }

  [HttpPost]
  [Topic("pubsub", "throughput-test-step-1", false)]
  public async Task ThroughputTest([FromBody] ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: MoveValidator.TestController.ThroughputTest"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    await _client.PublishEventAsync("pubsub", "throughput-test-step-2", dto);
  }
}
