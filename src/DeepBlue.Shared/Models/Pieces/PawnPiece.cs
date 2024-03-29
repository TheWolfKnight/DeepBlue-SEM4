
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class PawnPiece : PieceBase
{
  private readonly Sets _set;

  public override Sets PieceSet
  {
    get => _set;
    init => _set = value;
  }

  private readonly Move _moves = new Move(
    new List<List<MoveCommand>> {
          new(){MoveCommand.UP, MoveCommand.UP},
          new(){MoveCommand.UP}
        });

  private readonly Move _attacks = new Move(
        new List<List<MoveCommand>> {
          new(){MoveCommand.UP, MoveCommand.LEFT},
          new(){MoveCommand.UP, MoveCommand.RIGHT}
          }
      );

  public PawnPiece(Sets set)
  {
    _set = set;
  }

  public override bool IsValidMove(Move move)
  {
    throw new NotImplementedException();
  }
}
