
using Dapr;
using DeepBlue.Api.Gateway.Hubs;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DeepBlue.Api.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
  private readonly IHubContext<TestHub> _testHubContext;

  public TestController(IHubContext<TestHub> hubContext)
  {
    _testHubContext = hubContext;
  }

  [Topic("pubsub", "throughput-test-step-3")]
  public async Task TestThroughtputEndAsync(ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: Gateway.TestController.TestThroughputEndAsync"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    await _testHubContext.Clients.Client(dto.ConnectionId).SendAsync("TestThroughputEndAsync", dto);
  }
}
