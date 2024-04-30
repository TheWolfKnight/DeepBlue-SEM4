
using Dapr;
using DeepBlue.Api.Gateway.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DeepBlue.Api.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MakeMoveController : ControllerBase
{
  private readonly IHubContext<MakeMoveHub> _moveHubContext;

  public MakeMoveController(IHubContext<MakeMoveHub> moveHubContext)
  {
    _moveHubContext = moveHubContext;
  }

  [Topic("pubsub", "send-move-to-client")]
  public async Task SendMoveToClientAsync()
  {
    //TODO: this
    await _moveHubContext.Clients.Client("").SendAsync("");
  }
}
