
using DeepBlue.Api.MoveValidator.Services;
using DeepBlue.Shared.Models.Dtos;

namespace DeepBlue.Tests;

public class ValidationTests
{
  [Fact]
  public void WhitePieceStart_ValidMove_ValidSet_Test()
  {
    // Arrange
    FENService service = new FENService();

    string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    Point from = new Point(0, 6);
    Point to = new Point(0, 4);

    ValidateMoveDto dto = new ValidateMoveDto
    {
      FEN = fen,
      From = from,
      To = to,
      ConnectionId = "unused"
    };

    // Act
    bool result = service.IsValidMove(dto);

    // Assert
    Assert.True(result);
  }

  [Fact]
  public void BlackPieceStart_ValidMove_InvalidSet_Test()
  {
    // Arrange
    FENService service = new FENService();

    string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
    Point from = new Point(0, 1);
    Point to = new Point(0, 3);

    ValidateMoveDto dto = new ValidateMoveDto
    {
      FEN = fen,
      From = from,
      To = to,
      ConnectionId = "unused"
    };

    // Act
    bool result = service.IsValidMove(dto);

    // Assert
    Assert.False(result);
  }

  [Fact]
  public void BlackPieceStart_ValidMove_ValidSet_Test()
  {
    // Arrange
    FENService service = new FENService();

    string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR b KQkq - 0 1";
    Point from = new Point(0, 1);
    Point to = new Point(0, 3);

    ValidateMoveDto dto = new ValidateMoveDto
    {
      FEN = fen,
      From = from,
      To = to,
      ConnectionId = "unused"
    };

    // Act
    bool result = service.IsValidMove(dto);

    // Assert
    Assert.True(result);
  }
}
