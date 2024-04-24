
using DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeepBlue.Blazor.Features.HubConnectionFeature;

public class MakeMoveHubConnection : IMakeMoveHubConnection
{

  private readonly HubConnection _hubConnection;

  public bool IsConnected { get => _hubConnection.State is HubConnectionState.Connected; }

  public MakeMoveHubConnection()
  {
    _hubConnection = new HubConnectionBuilder()
      .WithUrl("https://localhost:5222/makemovehub")
      .Build();
  }

  public async Task StartAsync() => await _hubConnection.StartAsync();

  public void BindResultMethod(Action<MoveResultDto> action)
  {
    //TODO: check for the connection id, then make sure that it is the same
    //TODO: figure out how to let this connection live on
    _hubConnection.On<MoveResultDto>("MakeMove", (dto) =>
    {
      action(dto);
    });
  }

  public async Task MakeMoveAsync(string fen, Point p1, Point p2)
  {
    ValidateMoveDto payload = new ValidateMoveDto
    {
      FEN = fen,
      From = p1,
      To = p2,
      ConnectionId = _hubConnection.ConnectionId ?? string.Empty
    };

    await _hubConnection.SendAsync("ValidateMove", payload);
  }

  public async ValueTask DisposeAsync()
  {
    await _hubConnection.DisposeAsync();
  }
}
