
using DeepBlue.Shared.Models;

namespace DeepBlue.Blazor.Helpers;

public static class EnumerableHelpers
{

  public static PieceBase GetPiece(this IList<IList<PieceBase>> self, int x, int y)
  {
    return self.ElementAt(y).ElementAt(x);
  }

}
