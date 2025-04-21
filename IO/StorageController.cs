using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using MessagePack;
using static Starfall.Core.CreatePlayer; // 추가 by. 최영임

namespace Starfall.IO
{
    public static class StorageController
    {
        public static string savePath = "./saves";
        public static string saveName { get; private set; } = "default";
        public static JobName saveJob { get; private set; } = JobName.None;
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

        // ========================================
        // 추가 by. 최영임

        public static void SetSaveJob(JobName job)
        {
            CreateDir($"saves/world/{saveName}/{GetJobNameToKor(job)}");
            saveJob = job;
        }

        // ========================================
        // ========================================

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
            File.WriteAllText(Path.Combine("./saves/world/", saveName, path + ".json"), jsonString);
        }

        public static bool LoadObj<T>(string path, out T? obj)
        {
            obj = default;

            try
            {
                obj = JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine("./saves/world/", saveName, path + ".json")));
            }
            catch (JsonException)
            {
                return false;
            }

            return true;
        }

        public static void SaveBinary<T>(string name, T value)
        {
            byte[] data = MessagePackSerializer.Serialize(value);
            using var stream = new FileStream(Path.Combine("./saves/world/", saveName, name + ".bin"), FileMode.Create, FileAccess.Write);
            stream.Write(data, 0, data.Length);
            stream.Close();
        }

        public static T LoadBinary<T>(string name)
        {
            using var stream = new FileStream(Path.Combine("./saves/world/", saveName, name + ".bin"), FileMode.Open, FileAccess.ReadWrite);

            return MessagePackSerializer.Deserialize<T>(stream);
        }
    }
}