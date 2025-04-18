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
    public static string saveName { get; private set; } = "default";
    private static readonly Assembly assembly = Assembly.GetExecutingAssembly();
    private static readonly JsonSerializerOptions jsonOption = new()
    {
      Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
      WriteIndented = true
    };

    public static void Init()
    {
      CreateDir("saves");
      CreateDir("saves/world");
    }

    public static void SetSaveName(string name)
    {
      CreateDir($"saves/world/{saveName}");
      saveName = name;
    }

    public static string[] GetSaveNames()
    {
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

    private static void CreateDir(string path)
    {
      var info = new DirectoryInfo(path);
      if (!info.Exists) info.Create();
    }

    public static void SaveObj(string path, object data)
    {
      var jsonString = JsonSerializer.Serialize(data, jsonOption);
      File.WriteAllText(Path.Combine("./saves/world/", saveName, path), jsonString);
    }

    public static bool LoadObj<T>(string path, out T? obj)
    {
      obj = default;

      try
      {
        obj = JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine("./saves/world/", saveName, path)));
      }
      catch (JsonException)
      {
        return false;
      }

      return true;
    }

    public static void GetStream(string path)
    {

    }
  }
}