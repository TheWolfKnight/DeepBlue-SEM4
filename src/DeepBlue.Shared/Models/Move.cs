
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models;

public class Move
{
  public readonly List<List<MoveCommand>> MoveCommands = new();

  public Move(List<List<MoveCommand>> validMoves)
  {
    MoveCommands = validMoves;
  }
}
