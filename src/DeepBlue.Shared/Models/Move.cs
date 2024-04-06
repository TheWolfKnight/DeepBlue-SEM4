
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models;

public class Move
{
    public readonly MoveCommand[][] MoveCommands = [];

    public Move(MoveCommand[][] validMoves)
    {
        MoveCommands = validMoves;
    }
}
