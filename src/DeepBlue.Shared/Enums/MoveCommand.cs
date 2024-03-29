
namespace DeepBlue.Shared.Enums;

public enum MoveCommand
{
  UP = -1,
  DOWN = 1,
  LEFT = -1,
  RIGHT = 1,

  //NOTE: Used to repeat a collection of actions, making it easy to
  //      create the rook, bishop, and queen
  REPEATE = 2,
}
