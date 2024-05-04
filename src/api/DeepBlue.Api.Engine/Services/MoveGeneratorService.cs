
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
    return GenerateRandomMove(dto);
  }

  private int CalculateBoardValue(string boardStateFEN)
  {
    return 0;
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
      pieces = pieces.Concat(rank.Where(piece => piece is not EmptyPiece && piece.PieceSet == movingSet));

    Point choosenMove;
    PieceBase piece;

    while (true)
    {
      piece = pieces.ElementAt(rng.Next(0, pieces.Count()));

      int[,] validMoves = piece.GetValidMoves(boardState);
      List<Point> moves = new List<Point>();
      for (int x = 0; x < 8; ++x)
      {
        for (int y = 0; y < 8; ++y)
        {
          Console.WriteLine("X: " + x + " Y: " + y);
          if (validMoves[x, y] is not 0)
            moves.Add(new Point(X: x, Y: y));
        }
      }

      try
      {
        choosenMove = moves[rng.Next(0, moves.Count)];
        break;
      }
      catch (Exception)
      {

      }
    }

    Console.WriteLine($"Moving: {(piece.PieceSet is Sets.White ? "w" : "b")} {piece.GetType().Name} from X: {piece.Position[0]} Y: {piece.Position[1]}\nTo X: {choosenMove.X} Y: {choosenMove.Y}");

    boardState[choosenMove.Y][choosenMove.X] = boardState[piece.Position[1]][piece.Position[0]];
    boardState[piece.Position[1]][piece.Position[0]] = new EmptyPiece();

    return new MoveResultDto
    {
      FEN = FENHelpers.ConvertGameToFEN(boardState, movingSet is Sets.White ? Sets.Black : Sets.White),
      ConnectionId = dto.ConnectionId,
    };
  }
}
