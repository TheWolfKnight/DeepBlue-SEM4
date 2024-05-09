
using System;
using System.Security.Cryptography;
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
    return GenerateRandomMove(dto);
  }

  private int CalculateBoardValue(IList<IList<PieceBase>> boardState)
  {
    Func<PieceBase, int> getPieceValue = (PieceBase piece) =>
    {
      if (piece is EmptyPiece)
        return 0;

      return piece switch
      {
        KingPiece => 0,
        QueenPiece => 9,
        RookPiece => 5,
        BishopPiece or KnightPiece => 3,
        PawnPiece => 1,
        _ => throw new Exception("Unrechable code"),
      } * (int)piece.PieceSet;
    };

    return boardState.Sum(rank => rank.Sum(getPieceValue));
  }

  private IList<IList<PieceBase>> MakeMove(IList<IList<PieceBase>> boardState, PieceBase piece, Point to)
  {
    throw new NotImplementedException();
  }

  private MoveResultDto GenerateRandomMove(MakeMoveDto dto)
  {
    IList<IList<PieceBase>> boardState = FENHelpers.FENToBoard(dto.FENString);
    Sets movingSet = FENHelpers.GetMovingSetFromFEN(dto.FENString);

    Random rng = new Random(Guid.NewGuid().GetHashCode());

    IEnumerable<PieceBase> pieces = new List<PieceBase>();
    foreach (IList<PieceBase> rank in boardState)
      pieces = pieces.Concat(
        rank
        .Where(piece => piece is not EmptyPiece && piece.PieceSet == movingSet)
        .Where(piece => piece.PieceHasValidMove(boardState))
      );

    if (!pieces.Any())
      return new MoveResultDto
      {
        ConnectionId = dto.ConnectionId,
        FEN = dto.FENString
      };

    Point choosenMove;
    PieceBase piece;

    piece = pieces.ElementAt(rng.Next(0, pieces.Count()));

    int[,] validMoves = piece.GetValidMoves(boardState);
    List<Point> moves = new List<Point>();
    for (int x = 0; x < 8; ++x)
    {
      for (int y = 0; y < 8; ++y)
      {
        if (validMoves[x, y] is not 0)
          moves.Add(new Point(X: x, Y: y));
      }
    }

    choosenMove = moves[rng.Next(0, moves.Count)];

    boardState[choosenMove.Y][choosenMove.X] = boardState[piece.Position[1]][piece.Position[0]];
    boardState[piece.Position[1]][piece.Position[0]] = new EmptyPiece();

    return new MoveResultDto
    {
      FEN = FENHelpers.ConvertGameToFEN(boardState, movingSet is Sets.White ? Sets.Black : Sets.White),
      ConnectionId = dto.ConnectionId,
    };
  }

}
