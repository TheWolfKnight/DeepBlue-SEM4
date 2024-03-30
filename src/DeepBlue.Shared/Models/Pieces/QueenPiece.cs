
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class QueenPiece : PieceBase
{
  private readonly Sets _set;

  public override Sets PieceSet
  {
    get => _set;
    init => _set = value;
  }

  private readonly Move _moves = new Move(new List<List<MoveCommand>> {
      new(){MoveCommand.UP, MoveCommand.REPEATE},
      new(){MoveCommand.UP, MoveCommand.RIGHT, MoveCommand.REPEATE},
      new(){MoveCommand.RIGHT, MoveCommand.REPEATE},
      new(){MoveCommand.DOWN,MoveCommand.RIGHT, MoveCommand.REPEATE},
      new(){MoveCommand.DOWN, MoveCommand.REPEATE},
      new(){MoveCommand.DOWN, MoveCommand.LEFT, MoveCommand.REPEATE},
      new(){MoveCommand.LEFT, MoveCommand.REPEATE},
      new(){MoveCommand.UP, MoveCommand.LEFT, MoveCommand.REPEATE}
    });

  private readonly Move _attacks;

  public override Move Moves
  {
    get => _moves;
    init => _moves = value;
  }

  public override Move Attacks
  {
    get => _attacks;
    init => _attacks = value;
  }

  public QueenPiece(Sets set)
  {
    _set = set;
    _attacks = _moves;
  }

  public override MoveResult IsValidMove(Move move)
  {
    throw new NotImplementedException();
  }
}
