
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class RookPiece : PieceBase
{
  private readonly Sets _set;

  public override Sets PieceSet
  {
    get => _set;
    init => _set = value;
  }

  private readonly Move _moves = new Move(new List<List<MoveCommand>> {
      new(){MoveCommand.UP, MoveCommand.REPEATE},
      new(){MoveCommand.RIGHT, MoveCommand.REPEATE},
      new(){MoveCommand.DOWN, MoveCommand.REPEATE},
      new(){MoveCommand.LEFT, MoveCommand.REPEATE},
    });

  private readonly Move _attacks;

  public RookPiece(Sets set)
  {
    _set = set;
    _attacks = _moves;
  }

  public override bool IsValidMove(Move move)
  {
    throw new NotImplementedException();
  }
}
