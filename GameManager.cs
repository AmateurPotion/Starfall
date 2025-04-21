using System.Text.Json;
using Starfall.Core.Classic;
using Starfall.IO;
using Starfall.IO.Dataset;

namespace Starfall
{
  public static class GameManager
  {
    public static bool Loaded { get; private set; } = false;
    public static readonly Dictionary<string, ClassicItem> items = [];
    public static void Init()
    {
      if (Loaded) return;
      StorageController.Init();
      Console.Title = "StarFall";

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

      Loaded = true;
    }

    public static ClassicGame StartGame(GameData data)
    {
      var game = new ClassicGame(data);
      game.Start();

      return game;
    }

    public static void JoinGame()
    {

    }
  }
}