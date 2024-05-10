
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models;
public abstract class PieceBase
{
  public abstract Sets PieceSet { get; init; }
  public abstract int[] Position { get; set; }

  public abstract int[,] GetValidMoves(IEnumerable<IEnumerable<PieceBase>> board);
  public abstract int[,] GetAttackMoves(IEnumerable<IEnumerable<PieceBase>> board);

  public abstract void MadeMove();

  public override string ToString()
  {
    Sets? set = null;

    try
    {
      set = PieceSet;
    }
    catch (Exception) { }

    string setText = set is not null ? $"(Set={(set is Sets.White ? "w" : "b")})" : "";

    return $"{GetType().Name}{setText}";
  }
}
