
using System.ComponentModel;

namespace DeepBlue.Blazor.Models;

public class GameDisplay
{
  public int GameID { get; set; }

  [DisplayName("Last Played")]
  public DateTime LastPlayed { get; set; } = new DateTime();

  [DisplayName("FEN")]
  public string FENString { get; set; } = string.Empty;
}
