using System.Text.Json;
using Spectre.Console;
using Starfall.Contents.Binary;
using Starfall.Contents.Json;
using Starfall.Core;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.Contents;


namespace Starfall
{
	public static class GameManager
	{
		public static bool Loaded { get; private set; } = false;
		public static readonly Dictionary<string, Item> items = [];
		public static readonly Dictionary<string, MonsterData> monsters = [];
		public static readonly Dictionary<string, QuestData> quests = [];
		public static readonly Dictionary<string, Skill> skills = [];


		public static void Init()
		{
			if (Loaded) return;
			StorageController.Init();
			Console.Title = "StarFall";

			// Item Json 파일 불러오기
			foreach (var info in new DirectoryInfo("./Resources/items/").GetFiles())
			{
				try
				{
					if (info.Name.Contains(".json"))
					{
						var name = info.Name.Replace(".json", "");
						var stream = info.OpenRead();
						var data = JsonSerializer.Deserialize<Item>(stream);

						items[name] = data;

						stream.Close();
					}
				}
				catch (JsonException) { }

			}

			// foreach 문 넣었음   by. 박재현 
			foreach (var info in new DirectoryInfo("./Resources/monster/").GetFiles())
			{
				try
				{
					if (info.Name.Contains(".json"))
					{
						var name = info.Name.Replace(".json", "");
						var stream = info.OpenRead();
						var data = JsonSerializer.Deserialize<MonsterData>(stream);

						monsters[name] = data;

						stream.Close();
					}
				}
				catch (JsonException) { }
			}

			// 추가 by. 최영임
			// Quest Json 파일 불러오기
			//// json파일 필터 *json
			//foreach (var file in new DirectoryInfo("./Resources/quests/").GetFiles("*.json"))
			//{
			//    var name = Path.GetFileNameWithoutExtension(file.Name); // 확장자 제거

			//    if (!file.Exists)
			//        continue;

			//    using var stream = file.OpenRead(); // stream이 끝나면 자동 닫기

			//    var data = JsonSerializer.Deserialize<QuestData>(stream);

			//    if (data is not null) { quests[name] = data; }
			//    else { Console.WriteLine($"역직렬화 실패: {file.Name}"); }
			//}

			// 스킬 불러오기
			foreach (var info in new DirectoryInfo("./Resources/skills/").GetFiles())
			{
				try
				{
					if (info.Name.Contains(".json"))
					{
						var name = info.Name.Replace(".json", "");
						var stream = info.OpenRead();
						var data = JsonSerializer.Deserialize<Skill>(stream);

						skills[name] = data;

						stream.Close();
					}
				}

				catch (JsonException) { }

			}



			Loaded = true;
		}

		public static Game StartGame(GameData data)
		{
			var game = new Game(data);
			game.Start();

			return game;
		}

		public static void JoinGame()
		{

		}

		// 추가 by. 최영임 
		// Program.cs에서 가져옴. 
		public static void EnterMain()
		{
		Menu:
			Console.Clear();
			ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt");
			Console.WriteLine();

			StorageController.SetSaveName("default");
			switch (MenuUtil.OpenMenu("새로운 여정", "데이터 불러오기", "다른 여정 참여"))
			{
				case 0:
					// 새로운 여정 - 새 게임
					// 추가 by. 최영임 
					// 수정 by. 최영임 
					// 플레이어 첫 생성
					CreatePlayer.CreateNewPlayer();
					break;

				case 1:
					// 데이터 불러오기 - 데이터 불러오기
					Console.Clear();
					var menu = (from path in StorageController.GetSaveNames() select path.Replace("./saves/world\\", "")).ToArray();

					AnsiConsole.MarkupLine("불러올 데이터를 선택하세요. \n");
					var select = MenuUtil.OpenMenu([.. menu, "\n돌아가기"]);

					if (select > -1 && select < menu.Length)
					{
						StorageController.SetSaveName(menu[select]);
						GameManager.StartGame(StorageController.LoadBinary<GameData>("data"));
					}
					else goto Menu;
					break;

				case 2:
					// 다른 여정 참여 - 다른 여정 참여
					GameManager.JoinGame();
					Console.WriteLine("개발중입니다.");
					Console.ReadKey();
					goto Menu;

				case -1: goto Menu;
			}
		}
	}
}
