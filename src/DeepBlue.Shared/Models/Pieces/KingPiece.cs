
using DeepBlue.Shared.Enums;

namespace DeepBlue.Shared.Models.Pieces;

public class KingPiece : PieceBase
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

  public KingPiece(Sets set)
  {
    _set = set;
  }

  public override int[,] GetValidMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    int[,] result = new int[8, 8];

    Sets enemySet = _set is Sets.White ? Sets.Black : Sets.White;

    int[,] enemySight = CalculateEnemyMoves(board);

    for (int i = -1; i <= 1; ++i)
    {
      for (int j = -1; j <= 1; ++j)
      {
        int[] current_pos = [_position[0] + i, _position[1] + j];

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

        if (enemySight[current_pos[0], current_pos[1]] != 2)
          result[current_pos[0], current_pos[1]] = 1;
      }
    }

    return result;
  }

  public override int[,] GetAttackMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    int[,] result = new int[8, 8];

    Sets enemySet = _set is Sets.White ? Sets.Black : Sets.White;

    for (int i = -1; i <= 1; ++i)
    {
      for (int j = -1; j <= 1; ++j)
      {
        int[] current_pos = [_position[0] + i, _position[1] + j];

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
    }

    return result;
  }

  private int[,] CalculateEnemyMoves(IEnumerable<IEnumerable<PieceBase>> board)
  {
    Sets enemySet = _set is Sets.White ? Sets.Black : Sets.White;
    int[,] results = new int[8, 8];

    foreach (IEnumerable<PieceBase> pieces in board)
    {
      foreach (PieceBase piece in pieces)
      {
        if (piece is EmptyPiece || piece.PieceSet != enemySet)
          continue;

        int[,] pieceAttacks = piece.GetAttackMoves(board);

        for (int i = 0; i < 8; ++i)
        {
          for (int j = 0; j < 8; ++j)
          {
            if (results[j, i] == 2)
              continue;

            results[j, i] = pieceAttacks[j, i];
          }
        }
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
    _firstMove = false;
  }
}
