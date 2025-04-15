using Starfall.IO;
using Starfall.Worlds;

namespace Starfall
{
  public static class GameManager
  {
    public static bool loaded { get; private set; } = false;
    public static Dictionary<string, Tile> tiles = new();
    public static void Init()
    {
      if (loaded) return;
      StorageController.Init();
      foreach (var (name, data) in StorageController.LoadTileDataList())
      {
        tiles[name] = new()
        {
          code = data.Code,
          color = data.Color,
          background = data.Background,
          movable = data.movable
        };
      }

      loaded = true;
    }

    public static Game StartGame()
    {
      var game = new Game();

      return game;
    }

    public static void JoinGame()
    {

    }
  }
}