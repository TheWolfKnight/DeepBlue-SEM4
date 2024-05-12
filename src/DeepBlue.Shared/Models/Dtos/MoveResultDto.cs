
using System.Text.Json.Serialization;

namespace DeepBlue.Shared.Models.Dtos;

public class MoveResultDto
{
  [JsonPropertyName("fen")]
  public required string FEN { get; init; }

  public int Score { get; init; }

  public bool MoveWasValid { get; set; } = true;

  [JsonPropertyName("connectionId")]
  public required string ConnectionId { get; init; }
}
