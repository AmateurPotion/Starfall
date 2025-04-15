using Starfall.Contents;

namespace Starfall.PlayerService
{
  public class Player : Entity
  {
    public Player() : base('p')
    {
      this.color = ConsoleColor.Blue;
      this.position = new(0, 0);
    }
  }
}