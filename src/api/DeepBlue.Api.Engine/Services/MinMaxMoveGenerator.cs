
using System.Net.WebSockets;
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

  private int _bestEvaluationThisIteration;
  private Move _bestMoveThisIteration = new Move { From = new(-1, -1), To = new(-1, -1), Piece = new EmptyPiece(), MoveIsValid = false };

  public MoveResultDto GenerateMove(MakeMoveDto dto)
  {
    _board = new Board(dto.FENString);

    int score = Search(4);
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
      QueenPiece => 9,
      RookPiece => 5,
      BishopPiece or KnightPiece => 3,
      PawnPiece => 1,
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

  private int Search(int depth, int distFromRoot = 0, int alpha = int.MinValue, int beta = int.MaxValue)
  {
    if (_board is null)
      throw new InvalidOperationException("The board should not be null at this state, Unrechable code");

    if (depth is 0)
      return EvaluateBoardState();

    IEnumerable<Move> moves = GenerateAvaliableMoves();
    //TODO: check for checkmate

    if (moves.Count() is 0)
      return 0; //NOTE: Stalemate


    foreach (Move move in moves)
    {
      //NOTE: makes move on board
      _board.MakeMove(move);

      //NOTE: searches the new position for best move.
      //      alpha/beta is now seen from opponants side
      int evaluation = -Search(depth - 1, distFromRoot++, -alpha, -beta);

      //NOTE: removes the move from the board, to make ready for the next check
      _board.UnmakeMove(move);

      //NOTE: in this case, the move was so good that the opponant will not allow this position
      //      therefor it can be skipped
      if (evaluation >= beta)
        return beta;

      Console.WriteLine("=== Evaluation is " + evaluation);
      Console.WriteLine("=== alpha is " + alpha);
      Console.WriteLine("=== alpha is less than eval " + (evaluation > alpha));

      //NOTE: this is the new best move
      if (evaluation > alpha && distFromRoot is 0)
      {
        _bestEvaluationThisIteration = evaluation;
        _bestMoveThisIteration = move;
        alpha = evaluation;
      }
    }

    return alpha;
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

  private int EvaluateBoardState()
  {
    if (_board is null)
      throw new InvalidOperationException("The board should not be null at this state, Unrechable code");

    IList<IList<PieceBase>> boardState = _board.BoardState;

    Func<PieceBase, Sets, int> pieceValue = (PieceBase piece, Sets setToBeCounted) =>
    {
      if (piece is EmptyPiece || piece.PieceSet == setToBeCounted)
        return 0;
      return GetPieceValue(piece);
    };

    int blackMaterialWeight = boardState.Sum(rank => rank.Sum(piece => pieceValue(piece, Sets.Black)));
    int whiteMaterialWeight = boardState.Sum(rank => rank.Sum(piece => pieceValue(piece, Sets.White)));

    int eval = whiteMaterialWeight - blackMaterialWeight;

    int perspective = _board.MoveingSet is Sets.White ? 1 : -1;

    return eval * perspective;
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
