
using DeepBlue.Api.Engine.Enums;
using DeepBlue.Api.Engine.Services.Interfaces;

namespace DeepBlue.Api.Engine.Services;

public class MoveGeneratorFactory : IMoveGeneratorFactory
{
  private readonly IEnumerable<IMoveGeneratorService> _services;

  public MoveGeneratorFactory(IEnumerable<IMoveGeneratorService> services)
  {
    _services = services;
  }

  public IMoveGeneratorService GetMoveGeneratorService(GeneratorTypes type)
  {
    try
    {
      return _services.First(service => service.GeneratorType == type);
    }
    catch (Exception)
    {
      Console.WriteLine($"Failed to the find the Generator type {type.GetType().Name}, using random in sted");
      return new RandomMoveGeneratorService();
    }
  }
}
