
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;

namespace DeepBlue.Blazor.Helpers;

public static class PieceHelpers
{
    public static int[,] GetPieceMoves(PieceBase piece, int x, int y)
    {
        Move pieceMoveSet = piece.Moves;

        foreach (MoveCommand[] cmd in pieceMoveSet.MoveCommands)
        {
            throw new NotImplementedException();
        }

        int[,] result = new int[8, 8];

        return result;
    }
}
