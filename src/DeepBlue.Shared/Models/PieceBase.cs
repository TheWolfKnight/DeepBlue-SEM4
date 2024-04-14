
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models;
public abstract class PieceBase
{
  public abstract Sets PieceSet { get; init; }
  public abstract int[] Position { get; set; }

  public abstract int[,] GetValidMoves(IEnumerable<IEnumerable<PieceBase>> board);
  public abstract int[,] GetAttackMoves(IEnumerable<IEnumerable<PieceBase>> board);

  public abstract void MadeMove();
}
