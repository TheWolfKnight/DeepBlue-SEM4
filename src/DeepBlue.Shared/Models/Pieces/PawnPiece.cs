
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class PawnPiece : PieceBase
{
  private readonly Sets _set;
  private int[] _position = new int[2];
  private bool _firstMove = true;

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

  public PawnPiece(Sets set)
  {
    _set = set;
  }

  public override int[,] GetValidMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    int[,] results = new int[8, 8];

    for (int i = 1; i < 3 - (_firstMove ? 0 : 1); ++i)
    {
      int[] current_pos = [_position[0], _position[1] + -i * (_set is Sets.Black ? -1 : 1)];

      if (!MoveWithinBoard(current_pos[0], current_pos[1]))
        break;

      PieceBase piece = board.ElementAt(current_pos[1]).ElementAt(current_pos[0]);
      if (piece is EmptyPiece)
        results[current_pos[0], current_pos[1]] = 1;
      else
        break;
    }

    int[][] attacks = [[-1, -1 * (_set is Sets.Black ? -1 : 1)], [1, -1 * (_set is Sets.Black ? -1 : 1)]];
    Sets enemySet = _set is Sets.White ? Sets.Black : Sets.White;

    foreach (int[] attack in attacks)
    {
      int[] current_pos = [_position[0] + attack[0], _position[1] + attack[1]];

      if (!MoveWithinBoard(current_pos[0], current_pos[1]))
        continue;

      PieceBase piece = board.ElementAt(current_pos[1]).ElementAt(current_pos[0]);
      if (piece is not EmptyPiece && piece.PieceSet == enemySet)
        results[current_pos[0], current_pos[1]] = 2;
    }

    return results;
  }

  public override int[,] GetAttackMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    int[,] results = new int[8, 8];

    int[][] attacks = [[-1, -1 * (_set is Sets.Black ? -1 : 1)], [1, -1 * (_set is Sets.Black ? -1 : 1)]];

    foreach (int[] attack in attacks)
    {
      int[] current_pos = [_position[0] + attack[0], _position[1] + attack[1]];

      if (!MoveWithinBoard(current_pos[0], current_pos[1]))
        continue;

      PieceBase piece = board.ElementAt(current_pos[1]).ElementAt(current_pos[0]);
      if (piece is EmptyPiece)
        results[current_pos[0], current_pos[1]] = 2;
    }

    return results;
  }

  private bool MoveWithinBoard(int x, int y)
  {
    return x >= 0 && x < 8 && y >= 0 && y < 8;
  }

  public override void MadeMove()
  {
    _firstMove = false;
  }
}
