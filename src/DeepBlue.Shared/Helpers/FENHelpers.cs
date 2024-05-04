
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Shared.Helpers;

public static class FENHelpers
{
  public static Sets GetMovingSetFromFEN(string fen)
  {
    string[] choppedFEN = fen.Split(' ');

    if (choppedFEN.Length < 6)
      throw new InvalidOperationException("This FEN is not valid");

    return choppedFEN[1] switch
    {
      "w" => Sets.White,
      "b" => Sets.Black,
      _ => throw new InvalidDataException("The second position in a FEN must be either w or b"),
    };
  }

  public static IList<IList<PieceBase>> FENToBoard(string fenString)
  {
    string[] notationPieces = fenString.Split(' ');
    string[] ranks = notationPieces[0].Split('/');

    List<IList<PieceBase>> result = new List<IList<PieceBase>>();

    for (int i = 0; i < ranks.Length; ++i)
    {
      List<PieceBase> rank = new List<PieceBase>();

      int x = 0;

      for (int j = 0; j < ranks[i].Length; ++j)
      {
        char instruction = ranks[i][j];

        if (char.IsDigit(instruction))
        {
          int num = int.Parse(instruction.ToString());
          IList<PieceBase> emptySlots = PieceHelpers.GetEmptySlots(num);
          rank = rank
            .Concat(emptySlots)
            .ToList();
          x += num;
          continue;
        }

        PieceBase piece = PieceHelpers.GetPieceFromChar(instruction);
        piece.Position = [x, i];
        x++;
        rank.Add(piece);
      }

      result.Add(rank);
    }

    return result;
  }

  public static string ConvertGameToFEN(IList<IList<PieceBase>> board, Sets movingSet)
  {
    string[] ranks = new string[8];
    int rankPtr = 0;

    foreach (IList<PieceBase> row in board)
    {
      string rank = GetRankString(row);
      ranks[rankPtr++] = rank;
    }

    string result = string.Join("/", ranks);
    result += $" {(movingSet is Sets.White ? "w" : "b")} ";

    //NOTE: these elements are un-used in the current version
    result += "KQkq - 0 1";

    return result;
  }

  private static string GetRankString(IList<PieceBase> rank)
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
          n++;
        }
        result += n;
      }
      else
        result += piece.GetPieceLetter();
    }

    return result;
  }
}
