
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class QueenPiece : PieceBase
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

  public QueenPiece(Sets set)
  {
    _set = set;
  }

  public override int[,] GetValidMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    int[,] result = new int[8, 8];

    //NOTE: rotations for the bishop
    int[][] rots = [[-1, -1], [-1, 1], [1, -1], [1, 1], [0, -1], [-1, 0], [1, 0], [0, 1]];
    Sets enemySet = _set is Sets.White ? Sets.Black : Sets.White;

    foreach (int[] rot in rots)
    {
      int[] current_pos = [_position[0], _position[1]];

      while (true)
      {
        //NOTE: get the next position
        current_pos[0] += rot[0];
        current_pos[1] += rot[1];

        if (!MoveWithinBoard(current_pos[0], current_pos[1]))
          break;

        PieceBase boardPiece = board.ElementAt(current_pos[1]).ElementAt(current_pos[0]);

        if (boardPiece is EmptyPiece)
        {
          result[current_pos[1], current_pos[0]] = 1;
          continue;
        }
        else if (boardPiece.PieceSet == enemySet)
          result[current_pos[1], current_pos[0]] = 2;
        break;
      }
    }

    return result;
  }

  private bool MoveWithinBoard(int x, int y)
  {
    return x >= 0 && x < 8 && y >= 0 && y < 8;
  }
}
