
using DeepBlue.Shared.Models;

namespace DeepBlue.Blazor.Features.PieceFeature.Services.Interfaces;

public interface IPieceService
{
    public int[,] GetPieceMoves(PieceBase piece, int x, int y);
}
