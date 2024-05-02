
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;
using DeepBlue.Shared.Helpers;
using DeepBlue.Blazor.Features.HubConnectionFeature.Interfaces;
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Blazor.Models;

public class GameState
{
  private readonly IMakeMoveHubConnection _moveHubConnection;

  public GameState(IMakeMoveHubConnection moveHubConnection)
  {
    _moveHubConnection = moveHubConnection;
  }

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

  public async Task MakeMove(int x, int y)
  {
    if (SelectedPiece is null)
      return;

    int[] selectedPiecePosition = SelectedPiece.Position;
    BoardPieces[selectedPiecePosition[1]][selectedPiecePosition[0]] = new EmptyPiece();
    BoardPieces[y][x] = SelectedPiece;

    await _moveHubConnection.MakeMoveAsync(
      CurrentFEN,
      new Point(selectedPiecePosition[0], selectedPiecePosition[1]),
      new Point(x, y)
    );

    SelectedPiece.Position = [x, y];
    SelectedPiece.MadeMove();
    SelectedPiece = null;

    CanMovePieces = CanMovePieces is Sets.White ? Sets.Black : Sets.White;
    CurrentFEN = ConvertGameToFEN();
  }

  public string ConvertGameToFEN()
  {
    string[] ranks = new string[8];
    int rankPtr = 0;

    foreach (IList<PieceBase> row in BoardPieces)
    {
      string rank = GetRankString(row);
      ranks[rankPtr++] = rank;
    }

    string result = string.Join("/", ranks);
    result += $" {(CanMovePieces is Sets.White ? "w" : "b")} ";

    //NOTE: these elements are un-used in the current version
    result += "KQkq - 0 1";

    return result;
  }

  private string GetRankString(IList<PieceBase> rank)
  {
    string result = string.Empty;

    IEnumerator<PieceBase> columns = rank.GetEnumerator();

    while (columns.MoveNext())
    {
      PieceBase piece = columns.Current;

      if (piece is EmptyPiece)
      {
        int n = 1;
        while (columns.Current is EmptyPiece)
        {
          if (!columns.MoveNext())
          {
            result += n;
            return result;
          }
          else if (columns.Current is not EmptyPiece)
          {
            result += n;
            break;
          }

          n++;
        }
      }

      result += piece.GetPieceLetter();
    }

    return result;
  }
}
