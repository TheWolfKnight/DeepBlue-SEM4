
using DeepBlue.Blazor.Features.PieceFeature.Services.Interfaces;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;

namespace DeepBlue.Blazor.Features.PieceFeature.Services;

public class PieceService : IPieceService
{
    public int[,] GetPieceMoves(PieceBase piece, int x, int y)
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
