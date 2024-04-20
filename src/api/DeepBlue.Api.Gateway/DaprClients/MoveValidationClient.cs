

using Dapr.Client;
using DeepBlue.Api.DaprClients.Interfaces;
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Api.DaprClients;

public class MoveValidationClient : IMoveValidationClient
{
  private readonly DaprClient _daprClient;

  public MoveValidationClient()
  {
    _daprClient = new DaprClientBuilder().Build();
  }

  public async Task ValidateMove(ValidateMoveDto dto)
  {
    await _daprClient.PublishEventAsync("make-move-pubsub", "validation", dto);
  }

  public void Dispose()
  {
    _daprClient.Dispose();
  }
}
