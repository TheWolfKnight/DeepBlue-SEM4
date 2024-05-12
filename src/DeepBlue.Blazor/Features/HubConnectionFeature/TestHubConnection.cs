
using DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeepBlue.Blazor.Features.HubConnectionFeature;

public class TestHubConnection : ITestHubConnection
{
  private HubConnection? _hubConnection;

  public bool IsConnected { get => _hubConnection?.State is HubConnectionState.Connected; }

  public async Task StartAsync()
  {
    if (IsConnected)
      return;

    string url = Environment.GetEnvironmentVariable("services__gatewayservice__gatewayaccess__0") ?? "http://localhost:80";

    _hubConnection = new HubConnectionBuilder()
      .WithUrl($"{url}/testhub")
      .Build();

    _hubConnection.On<ThroughputTestDto>("TestThroughputEndAsync", TestThroughputEndAsync);

    await _hubConnection.StartAsync();
  }

  public async Task SendMessageAsync(string message)
  {
    if (_hubConnection is null || !IsConnected)
      return;

    await _hubConnection.SendAsync("TestEchoAsync", message);
  }

  public async Task TestThroughputAsync(string message)
  {
    if (_hubConnection is null || !IsConnected)
      return;

    var payload = new ThroughputTestDto
    {
      Message = message,
      ConnectionId = _hubConnection.ConnectionId ?? string.Empty,
    };

    await _hubConnection.SendAsync("TestThroughputAsync", payload);
  }

  public async Task TestThroughputEndAsync(ThroughputTestDto dto)
  {
    if (_hubConnection is null)
      return;

    if (_hubConnection.ConnectionId != dto.ConnectionId)
      return;

    await Task.Run(() => Console.WriteLine(dto.Message));
  }

  public async Task CountMovesTest(CountMovesDto dto)
  {
    if (_hubConnection is null || !IsConnected)
      return;

    await _hubConnection.SendAsync("CountMovesAsync", dto);
  }

  public async ValueTask DisposeAsync()
  {
    if (_hubConnection is null)
      return;

    await _hubConnection.DisposeAsync();
  }
}
