
using Dapr;
using Dapr.Client;
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

  [HttpPost]
  [Topic("pubsub", "generate-move")]
  public async Task GenerateMove(MakeMoveDto dto)
  {

    //TODO: this
    await _daprClient.PublishEventAsync("pubsub", "send-move-to-client");
  }
}