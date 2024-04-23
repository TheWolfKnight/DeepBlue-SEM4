
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;

namespace DeepBlue.Api.Gateway.Hubs;

[EnableCors(CorsPolicys.AllowFrontend)]
public class TestHub : Hub
{
  [EnableCors(CorsPolicys.AllowFrontend)]
  public async Task TestEcho(string message)
  {
    await Task.Run(() => Console.WriteLine(message));
  }
}
