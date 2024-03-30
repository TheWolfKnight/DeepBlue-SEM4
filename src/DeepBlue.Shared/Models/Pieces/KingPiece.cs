
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class KingPiece : PieceBase
{
  private readonly Sets _set;

  public override Sets PieceSet
  {
    get => _set;
    init => _set = value;
  }

  private readonly Move _moves = new Move(new List<List<MoveCommand>> {
      new(){MoveCommand.UP},
      new(){MoveCommand.UP, MoveCommand.RIGHT},
      new(){MoveCommand.RIGHT},
      new(){MoveCommand.DOWN,MoveCommand.RIGHT},
      new(){MoveCommand.DOWN},
      new(){MoveCommand.DOWN, MoveCommand.LEFT},
      new(){MoveCommand.LEFT},
      new(){MoveCommand.UP, MoveCommand.LEFT}
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

  public KingPiece(Sets set)
  {
    _set = set;
    _attacks = _moves;
  }

  public override MoveResult IsValidMove(Move move)
  {
    throw new NotImplementedException();
  }
}
