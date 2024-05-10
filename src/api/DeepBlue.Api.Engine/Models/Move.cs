using DeepBlue.Shared.Models;
using DeepBlue.Shared.Models.Dtos;
using DeepBlue.Shared.Models.Pieces;

namespace DeepBlue.Api.Engine.Models;

public class Move
{
  public required PieceBase Piece;
  public PieceBase CapturedPiece = new EmptyPiece();

  public required Point From;
  public required Point To;

  public override string ToString()
  {
    return $"Move(Piece={Piece}, CapturedPiece={CapturedPiece}, From=({From.X},{From.Y}), To=({To.X},{To.Y}))";
  }
}
