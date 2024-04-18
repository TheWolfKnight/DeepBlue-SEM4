
using System.Text.Json.Serialization;

namespace DeepBlue.Shared.Models.Dtos;

public record Point([property: JsonPropertyName("x")] int X, [property: JsonPropertyName("y")] int Y);

public class ValidateMoveDto
{
  [JsonPropertyName("from")]
  public required Point From { get; init; }
  [JsonPropertyName("to")]
  public required Point To { get; init; }

  [JsonPropertyName("fen")]
  public required string FEN { get; init; }
}
