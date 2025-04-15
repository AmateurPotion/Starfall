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
      File.WriteAllText(Path.Combine(savePath, "StoneTile.json")
        , JsonSerializer.Serialize(new TileData()
        {
          Code = '■',
          Color = ConsoleColor.Gray,
          Background = ConsoleColor.White,
          movable = true
        }, jsonOption));
      return Directory.GetDirectories("./saves/world");
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