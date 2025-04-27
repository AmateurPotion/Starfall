using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using MessagePack;
using Starfall.PlayerService;

namespace Starfall.IO
{
  public static class StorageController
  {
    public static string savePath = "./saves";
    public static string SaveName { get; private set; } = "default";
    public static Job SaveJob { get; private set; } = Job.None;
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
      SaveName = name;
      CreateDir($"saves/world/{SaveName}");
    }

    public static List<string> GetSaveNames()
    {
      var result = new List<string>();

      foreach (var dir in Directory.GetDirectories("./saves/world"))
      {
        var info = new DirectoryInfo(dir);
        foreach (var file in info.GetFiles())
        {
          if (file.Name == "data.bin")
            result.Add(dir);
        }
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

    private static void CreateDir(string path)
    {
      var info = new DirectoryInfo(path);
      if (!info.Exists)
      {
        Console.WriteLine(path);
        info.Create();
      }
    }

    public static void SaveJson(string path, object data)
    {
      var jsonString = JsonSerializer.Serialize(data, jsonOption);
      File.WriteAllText(Path.Combine("./saves/world/", SaveName, path + ".json"), jsonString);
    }

    public static bool LoadJson<T>(string path, out T? obj)
    {
      obj = default;

      try
      {
        obj = JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine("./saves/world/", SaveName, path + ".json")));
      }
      catch (JsonException)
      {
        return false;
      }

      return true;
    }

    public static void LoadJsonResources<T>(string path, Action<string, T> action)
    {
      foreach (var info in new DirectoryInfo(Path.Combine("./Resources/", path)).GetFiles())
      {
        try
        {
          if (info.Name.Contains(".json"))
          {
            var name = info.Name.Replace(".json", "");
            using var stream = info.OpenRead();
            var data = JsonSerializer.Deserialize<T>(stream);

            if (data is not null)
            {
              action(name, data);
            }
            else
            {
              Console.WriteLine($"역직렬화 실패: {info.Name}");
            }

            stream.Close();
          }
        }
        catch (JsonException ex)
        {
          Console.WriteLine($"json 파일 로드 실패 ({info.Name}): {ex.Message}");
        }
      }
    }

    public static void SaveBinary<T>(string name, T value)
    {
      byte[] data = MessagePackSerializer.Serialize(value);
      using var stream = new FileStream(Path.Combine("./saves/world/", SaveName, name + ".bin"), FileMode.Create, FileAccess.Write);
      stream.Write(data, 0, data.Length);
      stream.Close();
    }

    public static T LoadBinary<T>(string name)
    {
      using var stream = new FileStream(Path.Combine("./saves/world/", SaveName, name + ".bin"), FileMode.Open, FileAccess.ReadWrite);

      return MessagePackSerializer.Deserialize<T>(stream);
    }
  }
}