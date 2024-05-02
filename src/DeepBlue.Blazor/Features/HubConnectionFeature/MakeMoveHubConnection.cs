
using DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;
using DeepBlue.Shared.Models.Dtos;
using Microsoft.AspNetCore.SignalR.Client;

namespace DeepBlue.Blazor.Features.HubConnectionFeature;

public class MakeMoveHubConnection : IMakeMoveHubConnection
{
  private HubConnection? _hubConnection;

  private Action<MoveResultDto>? _onMoveResult;

  public bool IsConnected { get => _hubConnection?.State is HubConnectionState.Connected; }

  public async Task StartAsync()
  {
    if (IsConnected)
      return;

    string url = Environment.GetEnvironmentVariable("services__gateway-service__http__0") ?? "http://localhost:80";

    _hubConnection = new HubConnectionBuilder()
      .WithUrl($"{url}/makemovehub")
      .Build();

    await _hubConnection.StartAsync();
  }

  public void BindResultMethod(Action<MoveResultDto> action)
  {
    _onMoveResult = action;
  }

  public async Task MakeMoveAsync(string fen, Point p1, Point p2)
  {
    if (_hubConnection is null || !IsConnected)
      return;

    ValidateMoveDto payload = new ValidateMoveDto
    {
      FEN = fen,
      From = p1,
      To = p2,
      ConnectionId = _hubConnection.ConnectionId ?? string.Empty
    };

    await _hubConnection.SendAsync("ValidateMove", payload);
  }

  public void UpdateBoardState(MoveResultDto dto)
  {
    if (dto.ConnectionId != _hubConnection?.ConnectionId || _onMoveResult is null)
      return;

    _onMoveResult(dto);
  }

  public async ValueTask DisposeAsync()
  {
    if (_hubConnection is null)
      return;

    await _hubConnection.DisposeAsync();
  }
}
