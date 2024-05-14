
using System.ComponentModel.DataAnnotations;
using System.Net.WebSockets;
using DeepBlue.Api.Engine.Enums;
using DeepBlue.Api.Engine.Models;
using DeepBlue.Api.Engine.Services.Interfaces;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models.Pieces;
using Microsoft.AspNetCore.DataProtection;

namespace DeepBlue.Api.Engine.Services;

public class MinMaxMoveGenerator : IMoveGeneratorService
{
  public GeneratorTypes GeneratorType { get => GeneratorTypes.MinMax; }

  private Board? _board { get; set; }

  private int _bestEvaluationThisIteration;
  private Move _bestMoveThisIteration = new Move { From = new(-1, -1), To = new(-1, -1), Piece = new EmptyPiece(), MoveIsValid = false };

  private const int _instantCheckmateScore = 100_000;
  private const int _positiveInfinite = 9_999_999;
  private const int _negativeInfinite = -_positiveInfinite;

  public MoveResultDto GenerateMove(MakeMoveDto dto)
  {
    _board = new Board(dto.FENString);

    //NOTE: control the search depth
    int depth = 5;

    int score = Search(depth, 0, _negativeInfinite, _positiveInfinite);
    if (_bestMoveThisIteration.MoveIsValid)
      _board.MakeMove(_bestMoveThisIteration);

    Console.WriteLine("=== Best Score: " + score);
    Console.WriteLine("=== The best move was: " + _bestMoveThisIteration);
    Console.WriteLine("=== Sending FEN to user: " + _board.CurrentFEN);

    MoveResultDto result = new MoveResultDto
    {
      ConnectionId = dto.ConnectionId,
      FEN = _board.CurrentFEN,
      Score = score,
      MoveWasValid = _bestMoveThisIteration.MoveIsValid
    };

    return result;
  }

  public void CountMoves(CountMovesDto dto)
  {
    _board = new Board(dto.FEN);
    for (int i = 1; i <= 2; ++i)
    {
      Console.WriteLine("== depth: " + i);
      int result = CountMoveWithDepth(i);

      Console.WriteLine($"== Positions Counted: {result}");
    }
  }

  private int GetPieceValue(PieceBase piece)
  {
    if (piece is EmptyPiece)
      return 0;

    return piece switch
    {
      KingPiece => 0,
      QueenPiece => 900,
      RookPiece => 500,
      BishopPiece or KnightPiece => 300,
      PawnPiece => 100,
      _ => throw new Exception("Unrechable code"),
    };
  }

  private int CountMoveWithDepth(int depth)
  {
    if (_board is null)
      throw new InvalidOperationException("The board should not be null at this state, Unrechable code");

    if (depth is 0)
      return 1;

    IEnumerable<Move> moves = GenerateAvaliableMoves();

    int moveCount = 0;

    foreach (Move move in moves)
    {
      //NOTE: makes move on board
      _board.MakeMove(move);

      //NOTE: searches the new position for best move. If found; make this the new best evaluation
      moveCount += CountMoveWithDepth(depth - 1);

      //NOTE: removes the move from the board, to make ready for the next check
      _board.UnmakeMove(move);
    }

    return moveCount;
  }

  private int Search(int depth, int distFromRoot, int alpha, int beta)
  {
    if (_board is null)
      throw new InvalidOperationException();

    if (depth is 0)
      return EvaluateBoard();

    IEnumerable<Move> moves = GenerateAvaliableMoves();
    //NOTE: try to guess which moves are good, and put them up front to save time
    moves = PredictGoodMoves(moves);

    if (IsCheckmate())
    {
      //NOTE: can be controlled to make distant checkmates more important
      int correction = 1;
      int checkMateScore = _instantCheckmateScore - (correction * (depth - distFromRoot));
      return -checkMateScore;
    }

    //NOTE: is stalemate
    if (!moves.Any())
      return 0;

    foreach (Move move in moves)
    {
      //NOTE: Make the move on the board
      _board.MakeMove(move);
      //NOTE:_evaluate this new position, seen from the other sets perspective
      int evaluation = -Search(depth - 1, distFromRoot + 1, -beta, -alpha);
      //NOTE: unmake the board, to its original state
      _board.UnmakeMove(move);

      //NOTE: if the move is better than the beta, it is too good and the opponant will avoid it
      if (evaluation >= beta)
        return beta;

      //NOTE: if it is better than alpha, it will become the new alpha
      if (evaluation > alpha)
      {
        alpha = evaluation;

        //NOTE: the new best move has been found
        if (distFromRoot is 0)
        {
          _bestEvaluationThisIteration = evaluation;
          _bestMoveThisIteration = move;
        }
      }
    }

    return alpha;
  }

  /// <summary>
  /// Calaculate the value of the current board
  /// </summary>
  /// <returns> An integer representing the current board state </returns>
  /// <exception cref="InvalidOperationException"></exception>
  private int EvaluateBoard()
  {
    if (_board is null)
      throw new InvalidOperationException();

    Func<PieceBase, Sets, int> pieceValue = (PieceBase piece, Sets setToBeCounted) =>
    {
      if (piece is EmptyPiece || piece.PieceSet != setToBeCounted)
        return 0;
      return GetPieceValue(piece);
    };

    int whiteEval = _board.BoardState.Sum(rank =>
    {
      Func<PieceBase, int> selector = piece => pieceValue(piece, Sets.White);
      return rank.Sum(selector);
    });

    int blackEval = _board.BoardState.Sum(rank =>
    {
      Func<PieceBase, int> selector = piece => pieceValue(piece, Sets.Black);
      return rank.Sum(selector);
    });

    int eval = whiteEval - blackEval;
    int perspective = _board.MoveingSet is Sets.White ? 1 : -1;

    return eval * perspective;
  }

  private record MoveOrder(Move Move, int Weight);
  private IEnumerable<Move> PredictGoodMoves(IEnumerable<Move> generatedMoves)
  {
    List<MoveOrder> weighedMoves = new List<MoveOrder>();

    foreach (Move move in generatedMoves)
    {
      int moveScoreGuess = 0;

      //NOTE: score a move better, if you can capture a higher value piece
      if (move.CapturedPiece is not null)
        moveScoreGuess = 10 * GetPieceValue(move.CapturedPiece) - GetPieceValue(move.Piece);

      weighedMoves.Add(new MoveOrder(move, moveScoreGuess));
    }

    return weighedMoves
      .OrderByDescending(move => move.Weight)
      .Select(move => move.Move);
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

  /// <summary>
  /// For the purpose of this project, a checkmate is when the king is dead
  /// </summary>
  /// <returns> True if king dead, else false </returns>
  /// <exception cref="InvalidOperationException"></exception>
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
