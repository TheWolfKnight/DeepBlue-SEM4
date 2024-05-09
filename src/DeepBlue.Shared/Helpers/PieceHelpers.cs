using System.Reflection;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Shared.Helpers;

public static class PieceHelpers
{

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

  public static char GetPieceLetter(this PieceBase piece)
  {
    char chr = piece switch
    {
      KingPiece => 'K',
      QueenPiece => 'Q',
      RookPiece => 'R',
      BishopPiece => 'B',
      KnightPiece => 'N',
      PawnPiece => 'P',
      _ => throw new Exception("Unrechable code")
    };

    chr = piece.PieceSet is Sets.White ? chr : char.ToLower(chr);
    return chr;
  }

  public static string GetColor(Sets set)
  {
    return set is Sets.White ? "white" : "black";
  }

  public static List<PieceBase> GetEmptySlots(int num)
  {
    return Enumerable
      .Range(0, num)
      .Select(_ => (PieceBase)new EmptyPiece())
      .ToList();
  }

  public static PieceBase GetPieceFromChar(char piece)
  {
    ConstructorInfo constructor = GetPieceConstructor(piece);
    Sets set = char.IsUpper(piece) ? Sets.White : Sets.Black;
    return (PieceBase)constructor.Invoke([set]);
  }

  public static bool PieceHasValidMove(this PieceBase piece, IList<IList<PieceBase>> boardState)
  {
    int[,] validMoves = piece.GetValidMoves(boardState);

    for (int x = 0; x < 8; ++x)
    {
      for (int y = 0; y < 8; ++y)
      {
        if (validMoves[x, y] is not 0)
          return true;
      }
    }

    return false;
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
