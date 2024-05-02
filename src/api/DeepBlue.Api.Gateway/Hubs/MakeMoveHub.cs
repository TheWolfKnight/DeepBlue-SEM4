
using Microsoft.AspNetCore.Cors;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.SignalR;
using Dapr.Client;

namespace DeepBlue.Api.Gateway.Hubs;

[EnableCors]
public class MakeMoveHub : Hub
{
  private readonly DaprClient _daprClient = new DaprClientBuilder().Build();

  public async Task MakeMoveAsync(ValidateMoveDto dto)
  {
    await _daprClient.PublishEventAsync("pubsub", "validate-move", dto);
  }
}
