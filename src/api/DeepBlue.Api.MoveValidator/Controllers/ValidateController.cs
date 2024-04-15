
using Microsoft.AspNetCore.Mvc;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models;
using DeepBlue.Api.MoveValidator.Services.Interfaces;

namespace DeepBlue.Api.MoveValidator.Controllers;

[Controller]
[Route("[controller]")]
public class ValidatorController
{
  private readonly IFENService _fenService;

  public ValidatorController(IFENService fenService)
  {
    _fenService = fenService;
  }

  [HttpPut]
  [Route("validate")]
  public ActionResult ValidateMove(ValidateMoveDto dto)
  {
    IList<IList<PieceBase>> boardState = _fenService.FENToBoard(dto.FEN);

    PieceBase selectedPiece = boardState[dto.From.Y][dto.From.X];

    int[,] selectedPieceMoves = selectedPiece.GetValidMoves(boardState);

    if (selectedPieceMoves[dto.To.X, dto.To.Y] != 0)
      return new OkResult();
    return new StatusCodeResult(406);
  }
}
