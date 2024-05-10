
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Helpers;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Api.Engine.Models;

public class Board
{
  public string OriginalFEN { get; set; } = string.Empty;

  public IList<IList<PieceBase>> BoardState { get; set; } = Array.Empty<IList<PieceBase>>();
  public Sets MoveingSet { get; set; }

  //TODO: make attack mapes for opponant

  public Board(string fen)
  {
    OriginalFEN = fen;
    BoardState = FENHelpers.FENToBoard(fen);
    MoveingSet = FENHelpers.GetMovingSetFromFEN(fen);
  }

  public void MakeMove(Move move)
  {
    BoardState[move.To.Y][move.To.X] = move.Piece;
    BoardState[move.From.Y][move.From.X] = new EmptyPiece();

    move.Piece.Position = [move.To.X, move.To.Y];
    MoveingSet = MoveingSet is Sets.White ? Sets.Black : Sets.White;
  }

  public void UnmakeMove(Move move)
  {
    BoardState[move.From.Y][move.From.X] = move.Piece;
    BoardState[move.To.Y][move.To.X] = move.CapturedPiece;

    move.Piece.Position = [move.From.X, move.From.Y];
    MoveingSet = MoveingSet is Sets.White ? Sets.Black : Sets.White;
  }
}
