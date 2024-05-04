
using System;
using DeepBlue.Api.Engine.Services.Interfaces;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Helpers;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Api.Engine.Services;

public class MoveGeneratorService : IMoveGeneratorService
{
  public MoveResultDto GenerateMove(MakeMoveDto dto)
  {
    IList<IList<PieceBase>> boardState = FENHelpers.FENToBoard(dto.FENString);
    Sets movingSet = FENHelpers.GetMovingSetFromFEN(dto.FENString);

    Random rng = new Random(Guid.NewGuid().GetHashCode());

    while (true)
    {
      int x = rng.Next(0, 8);
      int y = rng.Next(0, 8);

      PieceBase piece = boardState[y][x];

      if (piece is EmptyPiece || piece.PieceSet != movingSet)
        continue;

      int[,] validMoves = piece.GetValidMoves(boardState);

      while (true)
      {
        x = rng.Next(0, 8);
        y = rng.Next(0, 8);

        if (validMoves[y, x] is 0)
          continue;

        return new MoveResultDto
        {
          //TODO: this
          FEN = string.Empty,
          ConnectionId = dto.ConnectionId,
        };
      }
    }
  }

  private int CalculateBoardValue(string boardStateFEN)
  {
    return 0;
  }

  private IList<IList<PieceBase>> MakeMove(IList<IList<PieceBase>> boardState, PieceBase piece, Point to)
  {
    throw new NotImplementedException();
  }
}
