
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models;
public abstract class PieceBase
{
  public abstract Sets PieceSet { get; init; }

  public abstract Move Moves { get; init; }
  public abstract Move Attacks { get; init; }

  public abstract MoveResult IsValidMove(Move move);
}
