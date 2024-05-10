
using DeepBlue.Api.Engine.Enums;
using DeepBlue.Api.Engine.Models;
using DeepBlue.Api.Engine.Services.Interfaces;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Api.Engine.Services;

public class MinMaxMoveGenerator : IMoveGeneratorService
{
  public GeneratorTypes GeneratorType { get => GeneratorTypes.MinMax; }

  private Board? _board { get; set; }

  public MoveResultDto GenerateMove(MakeMoveDto dto)
  {
    _board = new Board(dto.FENString);

    return new MoveResultDto
    {
      ConnectionId = dto.ConnectionId,
      FEN = dto.FENString
    };
  }

  public void CountMoves(MakeMoveDto dto)
  {
    _board = new Board(dto.FENString);
    for (int i = 1; i <= 2; ++i)
    {
      Console.WriteLine("depth: " + i);
      int result = PureSearch(i);

      Console.WriteLine($"== Positions Counted: {result}");
    }
  }

  private int CalculateBoardValue(IList<IList<PieceBase>> boardState)
  {
    int selector(PieceBase piece) => GetPieceValue(piece) * (int)piece.PieceSet;
    return boardState.Sum(rank => rank.Sum(selector));
  }

  private int GetPieceValue(PieceBase piece)
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
    };
  }

  private int PureSearch(int depth)
  {
    if (_board is null)
      throw new InvalidOperationException("The board should not be null at this state, Unrechable code");

    if (depth is 0)
      return 1; //TODO: Evaluate when here

    IEnumerable<Move> moves = GenerateAvaliableMoves();

    // if (moves.Count() is 0)
    // {
    //   //TODO: check for checkmate
    //   return 0; //NOTE: Stalemate
    // }

    int bestEvaluation = int.MinValue;
    int numPositions = 0;

    foreach (Move move in moves)
    {
      Console.WriteLine(move);
      //NOTE: makes move on board
      _board.MakeMove(move);

      //NOTE: searches the new position for best move. If found; make this the new best evaluation
      numPositions += PureSearch(depth - 1);
      // bestEvaluation = Math.Max(moveEvaluation, bestEvaluation);

      //NOTE: removes the move from the board, to make ready for the next check
      _board.UnmakeMove(move);
    }

    return numPositions;
  }

  private IEnumerable<Move> PredictGoodMoves(IEnumerable<Move> generatedMoves)
  {
    foreach (Move move in generatedMoves)
    {
      int moveScoreGuess = 0;

      //NOTE: score a move better, if you can capture a higher value piece
      if (move.CapturedPiece is not null)
        moveScoreGuess = 10 * GetPieceValue(move.CapturedPiece) - GetPieceValue(move.Piece);
    }
    throw new NotImplementedException();
  }

  private IEnumerable<Move> GenerateAvaliableMoves()
  {
    if (_board is null)
      throw new InvalidOperationException("The board should not be null at this state, Unrechable code");

    IEnumerable<Move> results = new List<Move>();

    foreach (IList<PieceBase> rank in _board.BoardState)
    {
      foreach (PieceBase piece in rank)
      {
        if (piece is EmptyPiece || piece.PieceSet != _board.MoveingSet)
          continue;

        results = results.Concat(GetPieceMoves(piece));
      }
    }

    return results;
  }

  private bool IsCheckmate()
  {
    if (_board is null)
      throw new InvalidOperationException("The board should not be null at this state, Unrechable code");

    IList<IList<PieceBase>> boardState = _board.BoardState;
    Sets movingSet = _board.MoveingSet;

    KingPiece? king = null;
    bool found = false;

    foreach (IList<PieceBase> rank in boardState)
    {
      if (found)
        break;

      foreach (PieceBase piece in rank)
      {
        if (piece is KingPiece kp && piece.PieceSet == movingSet)
        {
          king = kp;
          found = true;
          break;
        }
      }
    }

    //NOTE: if no king, the game is over
    if (king is null)
      return true;

    return false;
  }

  private List<Move> GetPieceMoves(PieceBase piece)
  {
    if (_board is null)
      throw new InvalidOperationException("The board should not be null at this state, Unrechable code");

    List<Move> results = new List<Move>();
    int[,] pieceMoves = piece.GetValidMoves(_board.BoardState);

    for (int x = 0; x < 8; ++x)
      for (int y = 0; y < 8; ++y)
      {
        if (pieceMoves[x, y] is 0)
          continue;

        Move move = new Move
        {
          Piece = piece,
          From = new Point(X: piece.Position[0], Y: piece.Position[1]),
          To = new Point(X: x, Y: y),
          CapturedPiece = _board.BoardState[y][x],
        };

        results.Add(move);
      }

    return results;
  }
}
