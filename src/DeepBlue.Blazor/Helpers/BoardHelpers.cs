using System.Reflection;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Blazor.Helpers;

public static class BoardHelpers
{

  public static int[,] GetPieceMoves(PieceBase piece, int x, int y)
  {
    throw new NotImplementedException();

  }

  public static string GetPieceString(PieceBase piece)
  {
    string result = piece switch
    {
      PawnPiece => "pawn_" + GetColor(piece.PieceSet) + ".png",
      KnightPiece => "knight_" + GetColor(piece.PieceSet) + ".png",
      RookPiece => "rook_" + GetColor(piece.PieceSet) + ".png",
      BishopPiece => "bishop_" + GetColor(piece.PieceSet) + ".png",
      QueenPiece => "queen_" + GetColor(piece.PieceSet) + ".png",
      KingPiece => "king_" + GetColor(piece.PieceSet) + ".png",
      _ => "",
    };

    return result;
  }

  public static IList<IEnumerable<PieceBase>> FENToBoard(string fenString, Sets set)
  {
    string[] notationPieces = fenString.Split(' ');
    string[] ranks = notationPieces[0].Split('/');

    List<IEnumerable<PieceBase>> result = new List<IEnumerable<PieceBase>>();

    for (int i = 0; i < ranks.Length; ++i)
    {
      List<PieceBase> rank = new List<PieceBase>();

      foreach (char instruction in ranks[i])
      {
        if (char.IsDigit(instruction))
        {
          int num = int.Parse(instruction.ToString());
          IList<PieceBase> emptySlots = GetEmptySlots(num);
          rank = rank
            .Concat(emptySlots)
            .ToList();
          continue;
        }

        PieceBase piece = GetPieceFromChar(instruction);
        rank.Add(piece);
      }

      result.Add(rank);
    }

    //NOTE: FEN is seen from whites side, so flip if black
    if (set is Sets.Black)
      result.Reverse();

    return result;
  }

  private static string GetColor(Sets set)
  {
    return set is Sets.White ? "white" : "black";
  }

  private static List<PieceBase> GetEmptySlots(int num)
  {
    return Enumerable
      .Range(0, num)
      .Select(_ => (PieceBase)new EmptyPiece())
      .ToList();
  }

  private static PieceBase GetPieceFromChar(char piece)
  {
    ConstructorInfo constructor = GetPieceConstructor(piece);
    Sets set = char.IsUpper(piece) ? Sets.White : Sets.Black;
    return (PieceBase)constructor.Invoke([set]);
  }

  private static ConstructorInfo GetPieceConstructor(char piece)
  {
    Type type = char.ToLower(piece) switch
    {
      'p' => typeof(PawnPiece),
      'r' => typeof(RookPiece),
      'n' => typeof(KnightPiece),
      'b' => typeof(BishopPiece),
      'q' => typeof(QueenPiece),
      'k' => typeof(KingPiece),
      _ => throw new Exception("Unrechable Code")
    };

    ConstructorInfo? constructor = type.GetConstructor([typeof(Sets)])
      ?? throw new Exception($"A constructor is missing from the piece: {piece}");

    return constructor;
  }
}
