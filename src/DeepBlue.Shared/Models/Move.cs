
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models;

public class Move
{
  public readonly MoveCommand[,] MoveCommands = new MoveCommand[0, 0];

  public Move(MoveCommand[,] validMoves)
  {
    MoveCommands = validMoves;
  }
}
