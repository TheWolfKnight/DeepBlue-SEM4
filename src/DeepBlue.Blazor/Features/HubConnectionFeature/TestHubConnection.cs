
using DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeepBlue.Blazor.Features.HubConnectionFeature;

public class TestHubConnection : ITestHubConnection
{
  private readonly HubConnection _hubConnection;

  public bool IsConnected { get => _hubConnection.State is HubConnectionState.Connected; }

  public TestHubConnection()
  {
    _hubConnection = new HubConnectionBuilder()
      .WithUrl("https://localhost:7057/testhub")
      .Build();
  }

  public async Task StartConnectionAsync()
  {
    if (IsConnected)
      return;

    await _hubConnection.StartAsync();
  }

  public async Task SendMessageAsync(string message)
  {
    if (_hubConnection is null || !IsConnected)
      return;

    await _hubConnection.SendAsync("TestEcho", message);
  }

  public async ValueTask DisposeAsync()
  {
    await _hubConnection.DisposeAsync();
  }
}
