
using Dapr;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using DeepBlue.Shared.Models.Dtos;
using Dapr.Client;
using DeepBlue.Api.Engine.Services.Interfaces;
using DeepBlue.Api.Engine.Services;

namespace DeepBlue.Api.Engine.Controllers;

[ApiController]
[Route("api/[controller]")]
[DisableCors]
public class TestController : ControllerBase
{
  private readonly DaprClient _client;
  private readonly IMoveGeneratorService _moveGenerator;

  public TestController(IMoveGeneratorFactory factory)
  {
    _client = new DaprClientBuilder().Build();
    _moveGenerator = factory.GetMoveGeneratorService(Enums.GeneratorTypes.MinMax);
  }

  [Topic("pubsub", "throughput-test-step-2")]
  [HttpPost("/throughput-test")]
  public async Task TestThroughputAsync(ThroughputTestDto dto)
  {
    await Task.Run(() => Console.WriteLine("=== From: Engine.TestController.TestThroughputAsync"));
    await Task.Run(() => Console.WriteLine($"=== Message: {dto.Message}"));

    await _client.PublishEventAsync("pubsub", "throughput-test-step-3", dto);
  }

  [Topic("pubsub", "count-moves-test")]
  [HttpPost("/count-test")]
  public async Task CountMovesTest(MakeMoveDto dto)
  {
    if (_moveGenerator is MinMaxMoveGenerator minMax)
      minMax.CountMoves(dto);
  }
}
