
using Dapr;
using Dapr.Client;
using DeepBlue.Api.Engine.Services.Interfaces;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DeepBlue.Api.Engine.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class GatewayController : ControllerBase
{

  private readonly DaprClient _daprClient = new DaprClientBuilder().Build();
  private readonly IMoveGeneratorService _moveGenerator;

  public GatewayController(IMoveGeneratorService moveGenerator)
  {
    _moveGenerator = moveGenerator;
  }

  [HttpPost]
  [Topic("pubsub", "generate-move")]
  public async Task GenerateMove(MakeMoveDto dto)
  {
    MoveResultDto result = _moveGenerator.GenerateMove(dto);

    await _daprClient.PublishEventAsync("pubsub", "send-move-to-client", result);
  }
}