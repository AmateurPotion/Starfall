using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Starfall.IO.Dataset;

namespace Starfall.IO
{
  public static class StorageController
  {
    public static string savePath = "./saves";
    private static readonly Assembly assembly = Assembly.GetExecutingAssembly();
    private static readonly JsonSerializerOptions jsonOption = new()
    {
      Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
      WriteIndented = true
    };

    public static void Init()
    {
      static void CreateDir(string path)
      {
        var info = new DirectoryInfo(path);
        if (!info.Exists) info.Create();
      }

      CreateDir("saves");
      CreateDir("saves/world");
    }

    public static void Save<T>(string name, T gameData) where T : IGameData
    {
      File.WriteAllText(Path.Combine(savePath, $"{name}.json")
        , JsonSerializer.Serialize(gameData, jsonOption));
    }

    public static string[] GetSaveNames()
    {
      return Directory.GetDirectories("./saves/world");
    }

    public static List<(string name, TileData data)> LoadTileDataList()
    {
      var result = new List<(string, TileData)>();

      foreach (var info in new DirectoryInfo("./Resources/tiles/").GetFiles())
      {
        try
        {
          if (info.Name.Contains("Tile.json"))
          {
            var name = info.Name.Replace("Tile.json", "");
            var stream = info.OpenRead();
            var data = JsonSerializer.Deserialize<TileData>(stream);

            result.Add((name, data));

            stream.Close();
          }
        }
        catch (JsonException)
        { }
      }

      return result;
    }

    public static bool TryGetResource(string path, out Stream stream)
    {
      stream = Stream.Null;

      if (assembly.GetManifestResourceStream(path) is Stream _stream)
      {
        stream = _stream;
        return true;
      }
      else
        return false;
    }
  }
}