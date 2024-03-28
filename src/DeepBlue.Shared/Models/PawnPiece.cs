
namespace DeepBlue.Shared.Models;

public class PawnPiece : PieceBase
{
  private readonly Move[] _moves = [];

  public override required Move[] ValidMoves { get => throw new NotImplementedException(); init => throw new NotImplementedException(); }

  public override bool IsValidMove(Move move)
  {
    throw new NotImplementedException();
  }
}
