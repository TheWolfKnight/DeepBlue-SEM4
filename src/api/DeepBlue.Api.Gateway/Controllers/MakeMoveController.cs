
using Dapr;
using DeepBlue.Api.Gateway.Hubs;
using DeepBlue.Shared.Models.Dtos;
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
  public async Task SendMoveToClientAsync(MakeMoveDto dto)
  {
    await _moveHubContext.Clients.Client(dto.ConnectionId).SendAsync("UpdateBoardState", dto);
  }
}
