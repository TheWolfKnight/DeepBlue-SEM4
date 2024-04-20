
using Dapr.Client;
using DeepBlue.Api.DaprClients.Interfaces;

namespace DeepBlue.Api.DaprClients;

public class MakeMoveClient : IMakeMoveClient
{
  private readonly DaprClient _daprClient;

  public MakeMoveClient()
  {
    _daprClient = new DaprClientBuilder().Build();
  }


  public void Dispose()
  {
    _daprClient.Dispose();
  }
}
