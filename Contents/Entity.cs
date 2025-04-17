using Starfall.Utils;

namespace Starfall.Contents
{
  public class Entity
  {
    public char code;
    public ConsoleColor color;
    public Vector2Int position;
    public event Action<Vector2Int> OnMove;

    public Entity(char code = '◈')
    {
      this.code = code;
      OnMove = (pos) => position = pos;
    }

    public static implicit operator char(Entity entity) => entity.code;
    public void Deconstruct(out char code, out ConsoleColor color)
    {
      code = this.code;
      color = this.color;
    }
  }
}