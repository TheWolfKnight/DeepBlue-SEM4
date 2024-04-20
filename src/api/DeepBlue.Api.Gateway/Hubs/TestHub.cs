
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace DeepBlue.Api.Gateway.Hubs;

[EnableCors(CorsPolicys.TestHub.AllowFrontend)]
public class TestHub : Hub
{
  public async Task TestEcho(string message)
  {
    await Task.Run(() => Console.WriteLine(message));
  }
}
