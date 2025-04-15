using Starfall.Utils;

namespace Starfall.Contents
{
  public class Entity(char code = '◈')
  {
    public char code = code;
    public ConsoleColor color;
    public Vector2Int position;

    public static implicit operator char(Entity entity) => entity.code;
    public void Deconstruct(out char code, out ConsoleColor color)
    {
      code = this.code;
      color = this.color;
    }
  }
}