
using Microsoft.AspNetCore.Cors;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace DeepBlue.Api.Gateway.Hubs;

[EnableCors]
public class MakeMoveHub : Hub
{

  public MakeMoveHub()
  {
  }

  public async Task MakeMove()
  {
    throw new System.NotImplementedException();
  }
}
