using Starfall.IO.Dataset;

namespace Starfall.Worlds
{
  public class Tile
  {
    public char code;
    public ConsoleColor color = ConsoleColor.White;
    public ConsoleColor background = ConsoleColor.Black;
    public bool movable = true;

    public Tile(char code = '◻')
    {
      this.code = code;
    }

    public Tile Duplicate()
    {
      return new Tile(code)
      {
        color = color,
        background = background,
        movable = movable
      };
    }

    public static implicit operator char(Tile tile) => tile.code;
    public static implicit operator Tile(TileData data)
      => new()
      {
        code = data.Code,
        color = data.Color,
        background = data.Background,
        movable = data.movable
      };
  }
}