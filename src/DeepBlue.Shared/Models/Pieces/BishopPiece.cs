
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class BishopPiece : PieceBase
{
  private readonly Sets _set;

  public override Sets PieceSet
  {
    get => _set;
    init => _set = value;
  }

  public readonly Move _moves = new Move(new List<List<MoveCommand>> {
      new(){MoveCommand.UP, MoveCommand.RIGHT, MoveCommand.REPEATE},
      new(){MoveCommand.DOWN,MoveCommand.RIGHT, MoveCommand.REPEATE},
      new(){MoveCommand.DOWN, MoveCommand.LEFT, MoveCommand.REPEATE},
      new(){MoveCommand.UP, MoveCommand.LEFT, MoveCommand.REPEATE}
    });

  private readonly Move _attacks;

  public BishopPiece(Sets set)
  {
    _set = set;
    _attacks = _moves;
  }

  public override bool IsValidMove(Move move)
  {
    throw new NotImplementedException();
  }
}