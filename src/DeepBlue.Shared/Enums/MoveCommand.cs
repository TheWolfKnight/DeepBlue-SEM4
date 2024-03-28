
namespace DeepBlue.Shared.Enums;

public enum MoveCommand
{
  UP = 0,
  DOWN,
  LEFT,
  RIGHT,

  //NOTE: Used to repeat a collection of actions, making it easy to
  //      create the rook, bishop, and queen
  REPEATE,
}
