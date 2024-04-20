
namespace DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;

public interface ITestHubConnection : IAsyncDisposable
{
  Task StartConnectionAsync();
  Task SendMessageAsync(string message);
}
