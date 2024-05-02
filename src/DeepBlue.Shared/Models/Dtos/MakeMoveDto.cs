
namespace DeepBlue.Shared.Models.Dtos;

public class MakeMoveDto
{
  public required string FENString { get; set; } = string.Empty;
  public required string ConnectionId { get; set; } = string.Empty;
}
