
using Microsoft.AspNetCore.Cors;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.SignalR;
using DeepBlue.Api.RedisHandler.Interfaces;

namespace DeepBlue.Api.Gateway.Hubs;

[EnableCors(CorsPolicies.AllowFrontend)]
public class MakeMoveHub : Hub
{
  private readonly IRedisHandler _redisHandler;

  public MakeMoveHub(IRedisHandler redisHandler)
  {
    _redisHandler = redisHandler;
  }

  public async Task MakeMove()
  {
    throw new System.NotImplementedException();
  }

  public async Task ValidateMove(ValidateMoveDto dto)
  {
    throw new System.NotImplementedException();
  }
}
