
using System.Reflection;
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Api.MoveValidator.Services;

public class FENService : IFENService
{
  public IList<IList<PieceBase>> FENToBoard(string fenString)
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

  private List<PieceBase> GetEmptySlots(int num)
  {
    return Enumerable
      .Range(0, num)
      .Select(_ => (PieceBase)new EmptyPiece())
      .ToList();
  }

  private PieceBase GetPieceFromChar(char piece)
  {
    ConstructorInfo constructor = GetPieceConstructor(piece);
    Sets set = char.IsUpper(piece) ? Sets.White : Sets.Black;
    return (PieceBase)constructor.Invoke([set]);
  }

  private ConstructorInfo GetPieceConstructor(char piece)
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
