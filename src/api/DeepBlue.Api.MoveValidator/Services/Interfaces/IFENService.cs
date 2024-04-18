
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;

namespace DeepBlue.Api.MoveValidator.Services.Interfaces;

public interface IFENService
{
  IList<IList<PieceBase>> FENToBoard(string fen);
  Sets GetMovingSetFromFEN(string fen);
}
