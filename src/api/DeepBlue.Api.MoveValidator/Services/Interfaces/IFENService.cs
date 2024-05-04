
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Api.MoveValidator.Services.Interfaces;

public interface IFENService
{
  bool IsValidMove(IList<IList<PieceBase>> boardState, Sets movingSet,
                   Point from, Point to);

  string GenerateNewFEN(IList<IList<PieceBase>> boardState, Sets movingSet, Point from, Point to);
}
