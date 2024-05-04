
using Dapr;
using DeepBlue.Api.Gateway.Hubs;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DeepBlue.Api.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class MakeMoveController : ControllerBase
{
  private readonly IHubContext<MakeMoveHub> _moveHubContext;

  public MakeMoveController(IHubContext<MakeMoveHub> moveHubContext)
  {
    _moveHubContext = moveHubContext;
  }

  [HttpPost]
  [Topic("pubsub", "send-move-to-client")]
  public async Task SendMoveToClientAsync(MoveResultDto dto)
  {
    await _moveHubContext.Clients.All.SendAsync("UpdateBoardState", dto);
  }
}
