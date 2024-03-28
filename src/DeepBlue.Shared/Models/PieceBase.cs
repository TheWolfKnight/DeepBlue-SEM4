
namespace DeepBlue.Shared.Models;
public abstract class PieceBase
{
  public required abstract Move[] ValidMoves { get; init; }

  public abstract bool IsValidMove(Move move);
}
