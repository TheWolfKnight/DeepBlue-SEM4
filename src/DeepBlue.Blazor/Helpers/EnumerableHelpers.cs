
using DeepBlue.Shared.Models;

namespace DeepBlue.Blazor.Helpers;

public static class EnumerableHelpers
{

    public static PieceBase GetPiece(this IEnumerable<IEnumerable<PieceBase>> self, int x, int y)
    {
        return self.ElementAt(y).ElementAt(x);
    }

}
