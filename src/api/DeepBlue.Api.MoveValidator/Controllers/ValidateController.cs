
using Microsoft.AspNetCore.Mvc;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models;
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Models.Pieces;
using DeepBlue.Shared.Enums;

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
    Sets movingSet = _fenService.GetMovingSetFromFEN(dto.FEN);

    PieceBase selectedPiece = boardState[dto.From.Y][dto.From.X];

    if (selectedPiece is EmptyPiece || selectedPiece.PieceSet != movingSet)
      return new StatusCodeResult(406);

    int[,] selectedPieceMoves = selectedPiece.GetValidMoves(boardState);

    if (selectedPieceMoves[dto.To.X, dto.To.Y] != 0)
      return new OkResult();
    return new StatusCodeResult(406);
  }
}
