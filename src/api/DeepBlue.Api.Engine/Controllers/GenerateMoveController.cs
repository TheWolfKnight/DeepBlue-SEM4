
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
public class GenerateMoveController : ControllerBase
{

  private readonly DaprClient _client;
  private readonly IMoveGeneratorService _moveGenerator;

  public GenerateMoveController(IMoveGeneratorService moveGenerator)
  {
    _client = new DaprClientBuilder().Build();
    _moveGenerator = moveGenerator;
  }

  [HttpPost]
  [Topic("pubsub", "generate-move")]
  public async Task GenerateMoveAsync(MakeMoveDto dto)
  {
    MoveResultDto result = _moveGenerator.GenerateMove(dto);
    await _client.PublishEventAsync("pubsub", "send-move-to-client", result);
  }
}