namespace Starfall.Utils
{
  public struct Vector2Int
  {
    public int x;
    public int y;

    public Vector2Int()
    {
      x = 0;
      y = 0;
    }

    public Vector2Int(int x = 0, int y = 0)
    {
      this.x = x;
      this.y = y;
    }

    public readonly void Deconstruct(out int x, out int y)
    {
      x = this.x;
      y = this.y;
    }

    public static readonly Vector2Int Up = new(0, 1);
    public static readonly Vector2Int Down = new(0, -1);
    public static readonly Vector2Int Left = new(-1, 0);
    public static readonly Vector2Int Right = new(1, 0);
  }
}