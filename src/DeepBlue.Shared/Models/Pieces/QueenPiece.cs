
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class QueenPiece : PieceBase
{
  private readonly Sets _set;
  private int[] _position = new int[2];

  public override Sets PieceSet
  {
    get => _set;
    init => _set = value;
  }
  public override int[] Position
  {
    get => _position;
    set => _position = value;
  }

  public QueenPiece(Sets set)
  {
    _set = set;
  }

  public override int[,] GetValidMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    throw new NotImplementedException();
  }
}
