
using DeepBlue.Blazor.Models;
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Helpers;
using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Tests;

public class FENTests
{
  private bool ComparePieces(PieceBase a, PieceBase b)
  {
    //NOTE: if both are empty pieces, return true
    if (a is EmptyPiece && b is EmptyPiece)
      return true;

    //NOTE: when one is an empty piece, but the other is not, return false
    if (a is not EmptyPiece && b is EmptyPiece || a is EmptyPiece && b is not EmptyPiece)
      return false;

    //NOTE: else compare types and sets
    return a.GetType() == b.GetType() && a.PieceSet == b.PieceSet;
  }

  [Fact]
  public void FENToBoard_Test()
  {
    // Arrange
    string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    IList<IList<PieceBase>> expectedResult = [
      [ new RookPiece(Sets.Black), new KnightPiece(Sets.Black), new BishopPiece(Sets.Black), new QueenPiece(Sets.Black), new KingPiece(Sets.Black), new BishopPiece(Sets.Black), new KnightPiece(Sets.Black), new RookPiece(Sets.Black) ],
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new PawnPiece(Sets.Black)).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new EmptyPiece()).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new EmptyPiece()).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new EmptyPiece()).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new EmptyPiece()).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new PawnPiece(Sets.White)).ToList(),
      [ new RookPiece(Sets.White), new KnightPiece(Sets.White), new BishopPiece(Sets.White), new QueenPiece(Sets.White), new KingPiece(Sets.White), new BishopPiece(Sets.White), new KnightPiece(Sets.White), new RookPiece(Sets.White) ],
    ];

    // Act
    IList<IList<PieceBase>> actualResult = BoardHelpers.FENToBoard(fen);

    // Assert
    bool areEqual = actualResult
      .Zip(expectedResult)
      .All(ranks =>
        ranks.First
          .Zip(ranks.Second)
          .All(column => ComparePieces(column.First, column.Second))
        );

    Assert.True(areEqual);
  }

  [Fact]
  public void BoardToFen_Test()
  {
    // Arrange
    IList<IList<PieceBase>> board = [
      [ new RookPiece(Sets.Black), new KnightPiece(Sets.Black), new BishopPiece(Sets.Black), new QueenPiece(Sets.Black), new KingPiece(Sets.Black), new BishopPiece(Sets.Black), new KnightPiece(Sets.Black), new RookPiece(Sets.Black) ],
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new PawnPiece(Sets.Black)).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new EmptyPiece()).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new EmptyPiece()).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new EmptyPiece()).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new EmptyPiece()).ToList(),
      Enumerable.Range(0, 8).Select(_ => (PieceBase)new PawnPiece(Sets.White)).ToList(),
      [ new RookPiece(Sets.White), new KnightPiece(Sets.White), new BishopPiece(Sets.White), new QueenPiece(Sets.White), new KingPiece(Sets.White), new BishopPiece(Sets.White), new KnightPiece(Sets.White), new RookPiece(Sets.White) ],
    ];

    GameState state = new GameState();
    state.BoardPieces = board;
    state.CanMovePieces = Sets.White;

    string expectedResult = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    // Act
    string actualResult = state.ConvertGameToFEN();

    // Assert
    Assert.Equal(expectedResult, actualResult);
  }
}