
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;

public interface ITestHubConnection : IAsyncDisposable
{
  Task StartAsync();
  Task SendMessageAsync(string message);
  Task TestThroughputAsync(string message);

  Task CountMovesTest(MakeMoveDto dto);
}
