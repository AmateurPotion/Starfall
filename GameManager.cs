using System.Text.Json;
using Spectre.Console;
using Starfall.Core.Classic;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.IO.Dataset;
using static Starfall.Core.CreatePlayer; // �߰� by. �ֿ���

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

        // �߰� by. �ֿ��� 
        // Program.cs���� ������. 
        public static void EnterMain()
        {
        Menu:
            Console.Clear();
            ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt", ConsoleColor.DarkMagenta, ConsoleColor.Green);
            Console.WriteLine();

            StorageController.SetSaveName("default");
            switch (MenuUtil.OpenMenu("���ο� ����", "������ �ҷ�����", "�ٸ� ���� ����"))
            {
                case 0:
                    // ���ο� ���� - �� ����
                    // �߰� by. �ֿ��� 
                    // ���� by. �ֿ��� 
                    // �÷��̾� ù ����
                    CreateNewPlayer();
                    break;

                case 1:
                    // ������ �ҷ����� - ������ �ҷ�����
                    Console.Clear();
                    var menu = (from path in StorageController.GetSaveNames() select path.Replace("./saves/world\\", "")).ToArray();

                    AnsiConsole.MarkupLine("�ҷ��� �����͸� �����ϼ���. \n");
                    var select = MenuUtil.OpenMenu([.. menu, "\n���ư���"]);

                    if (select > -1 && select < menu.Length)
                    {
                        StorageController.SetSaveName(menu[select]);
                        GameManager.StartGame(StorageController.LoadBinary<GameData>("data"));
                    }
                    else goto Menu;
                    break;

                case 2:
                    // �ٸ� ���� ���� - �ٸ� ���� ����
                    GameManager.JoinGame();
                    Console.WriteLine("�������Դϴ�.");
                    Console.ReadKey();
                    goto Menu;

                case -1: goto Menu;
            }
        }
    }
}