
using DeepBlue.Api.Engine.Enums;

namespace DeepBlue.Api.Engine.Services.Interfaces;

public interface IMoveGeneratorFactory
{
  IMoveGeneratorService GetMoveGeneratorService(GeneratorTypes type);
}
