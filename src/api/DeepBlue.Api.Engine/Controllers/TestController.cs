
using Dapr;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DeepBlue.Shared.Models.Dtos;
using Dapr.Client;

namespace DeepBlue.Api.Engine.Controllers;

[ApiController]
[Route("[controller]")]
[DisableCors]
public class TestController : ControllerBase
{
  private readonly DaprClient _client;

  public TestController()
  {
    _client = new DaprClientBuilder().Build();
  }

  [Topic("pubsub", "throughput-test-step-2")]
  [HttpPost]
  [Route("/throughputtest")]
  public async Task TestThroughputAsync(ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: Engine.TestController.TestThroughputAsync"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    await _client.PublishEventAsync("pubsub", "throughput-test-step-3", dto);
  }
}
