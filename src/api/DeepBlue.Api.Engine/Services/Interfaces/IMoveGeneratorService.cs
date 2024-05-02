
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Api.Engine.Services.Interfaces;

public interface IMoveGeneratorService
{
  MoveResultDto GenerateMove(MakeMoveDto dto);
}
