
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Helpers;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Api.MoveValidator.Services;

public class FENService : IFENService
{
  public bool IsValidMove(ValidateMoveDto dto)
  {
    IList<IList<PieceBase>> boardState = FENToBoard(dto.FEN);
    Sets movingSet = GetMovingSetFromFEN(dto.FEN);

    PieceBase selectedPiece = boardState[dto.From.Y][dto.From.X];

    if (selectedPiece is EmptyPiece || selectedPiece.PieceSet != movingSet)
      return false;

    int[,] selectedPieceMoves = selectedPiece.GetValidMoves(boardState);

    return selectedPieceMoves[dto.To.X, dto.To.Y] is not 0;
  }

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

  public string GenerateNewFEN(ValidateMoveDto dot)
  {
    throw new NotImplementedException();
  }
}
