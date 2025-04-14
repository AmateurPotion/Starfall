using Starfall.Entities;
using Starfall.Utils;
using Starfall.Worlds;

namespace Starfall.IO
{
  public class Renderer
  {
    public World world;
    public Player player;
    public int renderWidth, renderHeight;

    public Renderer(World world, Player player, int width = 10, int height = 10)
    {
      this.world = world;
      this.player = player;

      this.renderHeight = height;
      this.renderWidth = width;
    }

    public void Render(Vector2Int startPos)
    {
      var tiles = world.tileMap;
      var entities = world.entityMap;
      var (sx, sy) = startPos;
      var (consoleX, consoleY) = Console.GetCursorPosition();
      var renderArray = Enumerable.Repeat<(char code, ConsoleColor color, ConsoleColor background)>
        (('◻', ConsoleColor.White, ConsoleColor.Black), renderHeight * renderWidth).ToArray();

      for (int y = 0; y < renderHeight; y++)
      {
        for (int x = 0; x < renderWidth; x++)
        {
          var cell = renderArray[x + y * renderWidth];
          var index = x + sx + (y + sy) * world.width;
          var tile = tiles[index];

          cell.background = tile.background;

          if (entities[index] is var (code, color))
          {
            cell.code = code;
            cell.color = color;
          }
          else
          {
            cell.code = tile.code;
            cell.background = tile.background;
          }
        }
      }


      for (int y = 0; y < renderHeight; y++)
      {
        Console.SetCursorPosition(consoleX, consoleY + y);
        for (int x = 0; x < renderWidth; x++)
        {
          var (code, color, background) = renderArray[x + y * renderWidth];

          Console.BackgroundColor = background;
          Console.ForegroundColor = color;
          Console.Write(code);
        }
      }

      Console.ResetColor();
      Console.WriteLine();
    }
  }
}