
namespace DeepBlue.Shared.Models.Dtos;

public record Point(int X, int Y);

public class ValidateMoveDto
{
    public required Point From { get; init; }
    public required Point To { get; init; }

    public required string FEN { get; init; }
}
