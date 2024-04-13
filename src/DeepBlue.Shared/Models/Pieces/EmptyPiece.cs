
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class EmptyPiece : PieceBase
{
  public override Sets PieceSet
  {
    get => throw new NotImplementedException("The empty piece does not have a Set");
    init => throw new NotImplementedException("The empty piece should not have a Set");
  }

  public override int[] Position
  {
    get => throw new NotImplementedException("The empty piece does not have a Position");
    set => throw new NotImplementedException("The empty piece should not have a Position");
  }

  public override int[,] GetValidMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    return new int[8, 8];
  }
}
