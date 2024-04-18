
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Helpers;
using DeepBlue.Shared.Models;

namespace DeepBlue.Api.MoveValidator.Services;

public class FENService : IFENService
{
  public IList<IList<PieceBase>> FENToBoard(string fenString)
  {
    return BoardHelpers.FENToBoard(fenString);
  }

  public Sets GetMovingSetFromFEN(string fen)
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
}
