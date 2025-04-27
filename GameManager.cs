using Spectre.Console;
using Starfall.Contents.Binary;
using Starfall.Contents.Json;
using Starfall.Core;
using Starfall.Core.Quest;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.Contents;
using Starfall.PlayerService;


namespace Starfall
{
	public static class GameManager
	{
		public static bool Loaded { get; private set; } = false;
		#region Json
		public static readonly Dictionary<string, Item> items = [];
		public static readonly Dictionary<string, MonsterData> monsters = [];
		public static readonly Dictionary<string, QuestData> quests = [];
		public static readonly Dictionary<string, FloorData> floors = [];
		#endregion
		#region Loader
		public static readonly Dictionary<string, Skill> skills = [];
		public static readonly Dictionary<string, Event> events = [];
		public static readonly Dictionary<string, Dungeon> dungeons = [];
		#endregion
		public static Game? Instance { get; private set; }


		public static void Init()
		{
			if (Loaded) return;
			StorageController.Init();
			Console.Title = "StarFall";

			#region  JsonLoading
			// Item Json 파일 불러오기
			StorageController.LoadJsonResources<Item>("items", (name, item) =>
			{
				items[name] = item;
			});

			// 몬스터 json 불러오기
			// foreach 문 넣었음   by. 박재현 
			StorageController.LoadJsonResources<MonsterData>("monsters", (name, monster) =>
			{
				monsters[name] = monster;
			});

			// 퀘스트 불러오기 by. 최영임
			StorageController.LoadJsonResources<QuestJson>("quests", (name, raw) =>
			{
				quests[name] = QuestData.FromJson(raw);
			});

			// 층계 Json 불러오기
			// 다른 데이터 참조때문에 loader 보단 이전, Json들보단 끝에 로딩해야됩니다.
			StorageController.LoadJsonResources<FloorData>("floors", (name, data) =>
			{
				floors[name] = data;
			});
			#endregion

			// 컨텐츠로더는 구조상(스크립트기반 로더) 가장 마지막에 호출하게끔
			ContentLoader.Load();

			Loaded = true;
		}

		public static Game StartGame(GameData data)
		{
			Instance = new Game(data);
			Instance.Start();

			return Instance;
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
			switch (MenuUtil.OpenMenu("새로운 여정", "데이터 불러오기", "게임 종료"))
			{
				case 0:
					// 새로운 여정 - 새 게임
					// 추가 by. 최영임 
					// 수정 by. 최영임 
					// 플레이어 첫 생성
					Player.CreateNew();
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
						StartGame(StorageController.LoadBinary<GameData>("data"));
					}
					else goto Menu;
					break;

				case 2:
					break;

				case -1: goto Menu;
			}
		}
	}
}
