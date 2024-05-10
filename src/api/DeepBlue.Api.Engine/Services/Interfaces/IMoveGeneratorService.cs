
using DeepBlue.Api.Engine.Enums;
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Api.Engine.Services.Interfaces;

public interface IMoveGeneratorService
{
  public GeneratorTypes GeneratorType { get; }

  MoveResultDto GenerateMove(MakeMoveDto dto);
}
