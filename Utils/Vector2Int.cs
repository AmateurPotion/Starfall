namespace Starfall.Utils
{
  public struct Vector2Int
  {
    public int x;
    public int y;

    public Vector2Int(int x = 0, int y = 0)
    {
      this.x = x;
      this.y = y;
    }

    public void Deconstruct(out int x, out int y)
    {
      x = this.x;
      y = this.y;
    }
  }
}