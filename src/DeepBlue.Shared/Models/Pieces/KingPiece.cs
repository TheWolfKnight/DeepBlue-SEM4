
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

    private readonly Move _moves = new Move([
        [MoveCommand.UP],
        [MoveCommand.UP, MoveCommand.RIGHT],
        [MoveCommand.RIGHT],
        [MoveCommand.DOWN,MoveCommand.RIGHT],
        [MoveCommand.DOWN],
        [MoveCommand.DOWN, MoveCommand.LEFT],
        [MoveCommand.LEFT],
        [MoveCommand.UP, MoveCommand.LEFT]
    ]);

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
