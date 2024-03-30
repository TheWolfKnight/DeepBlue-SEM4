
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class EmptyPiece : PieceBase
{
  public override Sets PieceSet
  {
    get => throw new NotImplementedException("The empty piece does not have a Set");
    init => throw new NotImplementedException("The empty piece should not have a Set");
  }

  public override Move Moves
  {
    get => throw new NotImplementedException("The empty piece cannot make a move");
    init => throw new NotImplementedException("The empty piece should not have a moveset");
  }

  public override Move Attacks
  {
    get => throw new NotImplementedException("The empty piece cannot make an attack");
    init => throw new NotImplementedException("The empty piece should not have an attack pattern");
  }

  public override MoveResult IsValidMove(Move move)
  {
    return new MoveResult
    {
      MoveIsValid = false,
      ValidMove = [],
    };
  }
}
