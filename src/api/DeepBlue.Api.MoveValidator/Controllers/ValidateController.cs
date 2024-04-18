
using Microsoft.AspNetCore.Mvc;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models;
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Models.Pieces;
using DeepBlue.Shared.Enums;
using Dapr;

namespace DeepBlue.Api.MoveValidator.Controllers;

[Controller]
[Route("[controller]")]
public class ValidatorController : ControllerBase
{
  private readonly IFENService _fenService;

  public ValidatorController(IFENService fenService)
  {
    _fenService = fenService;
  }

  [HttpPut]
  [Route("validate")]
  public IResult ValidateMove([Topic("make-move-pubsub", "move-validater")] ValidateMoveDto dto)
  {
    IList<IList<PieceBase>> boardState = _fenService.FENToBoard(dto.FEN);
    Sets movingSet = _fenService.GetMovingSetFromFEN(dto.FEN);

    PieceBase selectedPiece = boardState[dto.From.Y][dto.From.X];

    if (selectedPiece is EmptyPiece || selectedPiece.PieceSet != movingSet)
      return Results.StatusCode(406);

    int[,] selectedPieceMoves = selectedPiece.GetValidMoves(boardState);

    if (selectedPieceMoves[dto.To.X, dto.To.Y] != 0)
      return Results.Ok();
    return Results.StatusCode(406);
  }
}
