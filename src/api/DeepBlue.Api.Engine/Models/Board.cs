
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Helpers;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Api.Engine.Models;

public class Board
{
  public string CurrentFEN { get; set; } = string.Empty;
  public IList<IList<PieceBase>> BoardState { get; set; } = Array.Empty<IList<PieceBase>>();
  public Sets MoveingSet { get; set; }

  //TODO: make attack mapes for opponant
  public int[,] EnemyAttacks;

  public Board(string fen)
  {
    CurrentFEN = fen;
    BoardState = FENHelpers.FENToBoard(fen);
    MoveingSet = FENHelpers.GetMovingSetFromFEN(fen);
  }

  public void MakeMove(Move move)
  {
    BoardState[move.To.Y][move.To.X] = move.Piece;
    BoardState[move.From.Y][move.From.X] = new EmptyPiece();


    move.Piece.MadeMove();

    move.Piece.Position = [move.To.X, move.To.Y];
    MoveingSet = MoveingSet is Sets.White ? Sets.Black : Sets.White;

    CurrentFEN = FENHelpers.ConvertGameToFEN(BoardState, MoveingSet);
  }

  public void UnmakeMove(Move move)
  {
    BoardState[move.From.Y][move.From.X] = move.Piece;
    BoardState[move.To.Y][move.To.X] = move.CapturedPiece;

    move.Piece.Position = [move.From.X, move.From.Y];
    MoveingSet = MoveingSet is Sets.White ? Sets.Black : Sets.White;

    CurrentFEN = FENHelpers.ConvertGameToFEN(BoardState, MoveingSet);
  }

  private void GenerateAttakcs()
  {
    foreach (IList<PieceBase> rank in BoardState)
    {
      foreach (PieceBase piece in rank)
      {

      }
    }
  }
}
