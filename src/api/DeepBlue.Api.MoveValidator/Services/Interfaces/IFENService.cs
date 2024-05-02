
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Api.MoveValidator.Services.Interfaces;

public interface IFENService
{
  bool IsValidMove(ValidateMoveDto dto);
  IList<IList<PieceBase>> FENToBoard(string fen);
  Sets GetMovingSetFromFEN(string fen);
  string GenerateNewFEN(ValidateMoveDto dto);
}
