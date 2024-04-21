
using Microsoft.AspNetCore.Cors;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.SignalR;
using DeepBlue.Api.DaprClients.Interfaces;
using DeepBlue.Api.RedisHandler.Interfaces;

namespace DeepBlue.Api.Gateway.Hubs;

[EnableCors(CorsPolicys.AllowFrontend)]
public class MakeMoveHub : Hub
{
  private readonly IMakeMoveClient _makeMoveClient;
  private readonly IMoveValidationClient _validationClient;
  private readonly IRedisHandler _redisHandler;

  public MakeMoveHub(IMakeMoveClient makeMoveClient, IMoveValidationClient validationClient, IRedisHandler redisHandler)
  {
    _makeMoveClient = makeMoveClient;
    _validationClient = validationClient;
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
