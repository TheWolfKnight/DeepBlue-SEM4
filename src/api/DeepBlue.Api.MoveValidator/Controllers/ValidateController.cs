
using Microsoft.AspNetCore.Mvc;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models;
using DeepBlue.Api.MoveValidator.Services.Interfaces;
using DeepBlue.Shared.Models.Pieces;
using DeepBlue.Shared.Enums;
using Dapr;
using Dapr.Client;
using Microsoft.AspNetCore.Cors;
using DeepBlue.Shared.Helpers;

namespace DeepBlue.Api.MoveValidator.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableCors]
public class ValidatorController : ControllerBase
{
  private readonly DaprClient _client;
  private readonly IFENService _fenService;

  public ValidatorController(IFENService fenService)
  {
    _client = new DaprClientBuilder().Build();
    _fenService = fenService;
  }

  [HttpPost]
  [Topic("pubsub", "validate-move", false)]
  public async Task ValidateMoveAsync(ValidateMoveDto dto)
  {
    Console.WriteLine(dto.FEN);

    IList<IList<PieceBase>> boardState = FENHelpers.FENToBoard(dto.FEN);
    Sets movingSet = FENHelpers.GetMovingSetFromFEN(dto.FEN);

    bool isValidMove = _fenService.IsValidMove(boardState, movingSet, dto.From, dto.To);

    //TODO: send result back, to roll back the board
    if (!isValidMove)
    {
      Console.WriteLine("Move was invalid");
      return;
    }

    Console.WriteLine("Move was valid");

    MakeMoveDto payload = new MakeMoveDto
    {
      ConnectionId = dto.ConnectionId,
      FENString = _fenService.GenerateNewFEN(boardState, movingSet is Sets.White ? Sets.Black : Sets.White, dto.From, dto.To),
    };

    await _client.PublishEventAsync("pubsub", "generate-move", payload);
  }
}
