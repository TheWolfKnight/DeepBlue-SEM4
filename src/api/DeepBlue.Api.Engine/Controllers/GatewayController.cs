
using Dapr;
using Dapr.Client;
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
  public async Task GenerateMove()
  {

    //TODO: this
    await _daprClient.PublishEventAsync("pubsub", "send-move-to-client");
  }
}