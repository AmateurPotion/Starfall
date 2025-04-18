using System.Text.Json;
using Starfall.Core;
using Starfall.IO;
using Starfall.IO.Dataset;
using Starfall.Worlds;

namespace Starfall
{
  public static class GameManager
  {
    public static bool loaded { get; private set; } = false;
    public static readonly Dictionary<string, Tile> tiles = [];
    public static readonly Dictionary<string, ClassicItem> items = [];
    public static void Init()
    {
      if (loaded) return;
      StorageController.Init();

      foreach (var info in new DirectoryInfo("./Resources/tiles/").GetFiles())
      {
        try
        {
          if (info.Name.Contains("Tile.json"))
          {
            var name = info.Name.Replace("Tile.json", "");
            var stream = info.OpenRead();
            var data = JsonSerializer.Deserialize<TileData>(stream);

            tiles[name] = data;

            stream.Close();
          }
        }
        catch (JsonException)
        { }
      }

      foreach (var info in new DirectoryInfo("./Resources/items/").GetFiles())
      {
        try
        {
          if (info.Name.Contains(".json"))
          {
            var name = info.Name.Replace(".json", "");
            var stream = info.OpenRead();
            var data = JsonSerializer.Deserialize<ClassicItem>(stream);

            items[name] = data;

            stream.Close();
          }
        }
        catch (JsonException)
        { }
      }

      loaded = true;
    }

    public static Game StartGame()
    {
      StorageController.SetSaveName("default");
      var data = new GameData();
      StorageController.SaveObj("data.json", data);
      var game = new Game(data);
      game.Start();

      return game;
    }

    public static void JoinGame()
    {

    }
  }
}