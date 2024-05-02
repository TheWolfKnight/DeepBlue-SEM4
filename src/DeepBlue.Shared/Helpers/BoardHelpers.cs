using System.Reflection;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Shared.Helpers;

public static class BoardHelpers
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
          IList<PieceBase> emptySlots = GetEmptySlots(num);
          rank = rank
            .Concat(emptySlots)
            .ToList();
          x += num;
          continue;
        }

        PieceBase piece = GetPieceFromChar(instruction);
        piece.Position = [x, i];
        x++;
        rank.Add(piece);
      }

      result.Add(rank);
    }

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
