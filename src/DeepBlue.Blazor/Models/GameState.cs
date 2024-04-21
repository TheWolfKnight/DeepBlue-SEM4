
using DeepBlue.Blazor.Views;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Blazor.Models;

public class GameState
{
  public string CurrentFEN { get; set; } = string.Empty;

  public IList<IList<PieceBase>> BoardPieces { get; set; } = new List<IList<PieceBase>>();

  public PieceBase? SelectedPiece { get; set; } = null;

  public Sets PlayerSet { get; } = Sets.White;

  public Sets CanMovePieces { get; set; } = Sets.White;

  public PieceBase GetPiece(int x, int y)
  {
    if (BoardPieces.Count() < y || BoardPieces.First().Count() < x)
      throw new InvalidOperationException("You must index at a valid point");

    return BoardPieces.ElementAt(y).ElementAt(x);
  }

  public void MakeMove(int x, int y)
  {
    if (SelectedPiece is null)
      return;

    int[] selectedPiecePosition = SelectedPiece.Position;
    BoardPieces[selectedPiecePosition[1]][selectedPiecePosition[0]] = new EmptyPiece();
    BoardPieces[y][x] = SelectedPiece;

    SelectedPiece.Position = [x, y];

    SelectedPiece.MadeMove();
    SelectedPiece = null;

    CanMovePieces = CanMovePieces is Sets.White ? Sets.Black : Sets.White;
  }

  public string ConvertGameToFEN()
  {
    string[] ranks = new string[8];
    int rankPtr = 0;

    foreach (IList<PieceBase> row in BoardPieces)
    {
      string rank = string.Empty;
      IEnumerator<PieceBase> columns = row.GetEnumerator();

      while (columns.MoveNext())
      {

      }

      ranks[rankPtr] = rank;
    }

    string result = string.Join("/", ranks);
    result += $" {(CanMovePieces is Sets.White ? "w" : "b")} ";

    result += " KQkq 0 1";

    return result;
  }
}
