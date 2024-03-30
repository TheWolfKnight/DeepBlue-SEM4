
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models;

public class MoveResult
{

  public required MoveCommand[] ValidMove { get; init; }

  public required bool MoveIsValid { get; init; }

}
