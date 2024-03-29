
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models;
public abstract class PieceBase
{
  public abstract Sets PieceSet { get; init; }

  public abstract bool IsValidMove(Move move);
}
