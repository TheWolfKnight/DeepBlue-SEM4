
using DeepBlue.Shared.Enums;
using DeepBlue.Shared.Models;

namespace DeepBlue.Blazor.Helpers;

public static class BoardHelpers
{

  public static PieceBase[,] GetDefaultBoard(Sets playerSet)
  {
    return playerSet == Sets.White ? WhitePlayerBoard() : BlackPlayerBoard();
  }

  private static PieceBase[,] BlackPlayerBoard()
  {
    throw new NotImplementedException();
  }

  private static PieceBase[,] WhitePlayerBoard()
  {
    throw new NotImplementedException();
  }

}
