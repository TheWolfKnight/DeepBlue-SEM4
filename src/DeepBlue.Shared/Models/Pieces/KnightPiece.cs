
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class KnightPiece : PieceBase
{
  private readonly Sets _set;
  private int[] _position = new int[2];

  public override Sets PieceSet
  {
    get => _set;
    init => _set = value;
  }
  public override int[] Position
  {
    get => _position;
    set => _position = value;
  }

  public KnightPiece(Sets set)
  {
    _set = set;
  }

  public override int[,] GetValidMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    int[,] result = new int[8, 8];

    int[][] moves = [[-1, -2], [-2, -1], [-1, 2], [-2, 1], [1, -2], [2, -1], [1, 2], [2, 1]];
    Sets enemySet = _set is Sets.White ? Sets.Black : Sets.White;

    foreach (int[] move in moves)
    {
      int[] current_pos = [_position[0] + move[0], _position[1] + move[1]];

      if (!MoveWithinBoard(current_pos[0], current_pos[1]))
        continue;

      PieceBase piece = board.ElementAt(current_pos[1]).ElementAt(current_pos[0]);

      if (piece is not EmptyPiece && piece.PieceSet == enemySet)
      {
        result[current_pos[0], current_pos[1]] = 2;
        continue;
      }
      else if (piece is not EmptyPiece)
        continue;

      result[current_pos[0], current_pos[1]] = 1;
    }

    return result;
  }

  public override int[,] GetAttackMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    int[,] results = GetValidMoves(board);

    for (int i = 0; i < 8; ++i)
    {
      for (int j = 0; j < 8; ++j)
      {
        if (results[j, i] is 1)
          results[j, i] = 2;
      }
    }

    return results;
  }

  private bool MoveWithinBoard(int x, int y)
  {
    return x >= 0 && x < 8 && y >= 0 && y < 8;
  }

  public override void MadeMove()
  {
  }
}
