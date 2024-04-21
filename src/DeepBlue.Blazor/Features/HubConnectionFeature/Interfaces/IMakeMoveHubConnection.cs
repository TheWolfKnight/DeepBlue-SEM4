
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;

public interface IMakeMoveHubConnection : IAsyncDisposable
{
  Task StartAsync();

  Task MakeMoveAsync(string fen, Point p1, Point p2);

  void BindResultMethod(Action<MoveResultDto> action);
}
