
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Api.DaprClients.Interfaces;

public interface IMoveValidationClient : IDisposable
{
  Task ValidateMove(ValidateMoveDto dto);
}
