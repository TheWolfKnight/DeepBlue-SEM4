
using System.Text.Json.Serialization;

namespace DeepBlue.Shared.Models.Dtos;

public class MoveResultDto
{
  [JsonPropertyName("fen")]
  public required string FEN { get; init; }

  [JsonPropertyName("connectionId")]
  public required string ConnectionId { get; init; }
}
