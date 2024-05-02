
using Microsoft.AspNetCore.Mvc;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models;
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Models.Pieces;
using DeepBlue.Shared.Enums;
using Dapr;
using Dapr.Client;

namespace DeepBlue.Api.MoveValidator.Controllers;

[Controller]
[Route("api/[controller]")]
public class ValidatorController : ControllerBase
{
  private readonly DaprClient _daprClient = new DaprClientBuilder().Build();
  private readonly IFENService _fenService;

  public ValidatorController(IFENService fenService)
  {
    _fenService = fenService;
  }

  [Topic("pubsub", "move-validater")]
  [HttpPut]
  [Route("validate")]
  public async Task<IResult> ValidateMove(ValidateMoveDto dto)
  {
    IList<IList<PieceBase>> boardState = _fenService.FENToBoard(dto.FEN);
    Sets movingSet = _fenService.GetMovingSetFromFEN(dto.FEN);

    PieceBase selectedPiece = boardState[dto.From.Y][dto.From.X];

    if (selectedPiece is EmptyPiece || selectedPiece.PieceSet != movingSet)
      return Results.StatusCode(406);

    int[,] selectedPieceMoves = selectedPiece.GetValidMoves(boardState);

    if (selectedPieceMoves[dto.To.X, dto.To.Y] is 0)
      return Results.StatusCode(406);

    MakeMoveDto payload = new MakeMoveDto
    {
      ConnectionId = dto.ConnectionId,
      FENString = _fenService.GenerateNewFEN(dto),
    };

    await _daprClient.PublishEventAsync("pubsub", "generate-move", payload);
    return Results.Ok();
  }
}
