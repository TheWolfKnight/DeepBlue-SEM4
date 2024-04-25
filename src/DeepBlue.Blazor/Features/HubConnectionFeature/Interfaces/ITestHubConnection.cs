
namespace DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;

public interface ITestHubConnection : IAsyncDisposable
{
  Task StartAsync();
  Task SendMessageAsync(string message);
  Task TestThroughputAsync(string message);
}
