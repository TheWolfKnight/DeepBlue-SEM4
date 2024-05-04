
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Helpers;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Api.MoveValidator.Services;

public class FENService : IFENService
{
  public bool IsValidMove(IList<IList<PieceBase>> boardState, Sets movingSet,
                          Point from, Point to)
  {
    PieceBase selectedPiece = boardState[from.Y][from.X];

    if (selectedPiece is EmptyPiece || selectedPiece.PieceSet != movingSet)
      return false;

    int[,] selectedPieceMoves = selectedPiece.GetValidMoves(boardState);

    return selectedPieceMoves[to.X, to.Y] is not 0;
  }

  public string GenerateNewFEN(IList<IList<PieceBase>> boardState, Sets movingSet,
                          Point from, Point to)
  {
    boardState[to.Y][to.X] = boardState[from.Y][from.X];
    boardState[from.Y][from.X] = new EmptyPiece();

    return FENHelpers.ConvertGameToFEN(boardState, movingSet);
  }
}
