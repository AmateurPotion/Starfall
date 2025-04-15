using Starfall.Contents;

namespace Starfall.Worlds
{
  public class World
  {
    public readonly int width, height;
    public readonly Tile[] tileMap;
    public readonly Entity?[] entityMap;

    public World(Tile baseTile, int sWidth = 100, int sHeight = 100)
    {
      this.width = sWidth;
      this.height = sHeight;
      this.tileMap = new Tile[width * height];
      this.entityMap = [.. Enumerable.Repeat<Entity?>(null, width * height)];

      for (int i = 0; i < tileMap.Length; i++)
      {
        tileMap[i] = baseTile.Duplicate();
      }
    }

    public void Save()
    {

    }
  }
}