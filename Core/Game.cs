using System.ComponentModel.Design;
using System.Text;
using Spectre.Console;
using Starfall.Contents;
using Starfall.Contents.Binary;
using Starfall.Contents.Json;
using Starfall.Core.Quest;
using Starfall.Core;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.PlayerService;

namespace Starfall.Core;
public class Game(GameData data)
{
	public Player player = data;
	public Shop shop = new("수련자갑옷", "무쇠갑옷", "스파르타의갑옷", "낡은검", "청동도끼", "스파르타의창", "마력의장신구", "체력의장신구", "체력포션", "마력포션");
	private Func<bool> act = () => false;
	private static Random random = new();

	public void Start()
	{
		act = OpenHub;
		// 메소드 스택오버플로우를 막기위해 for문으로 구현했다.
		for (bool exit = OpenHub(); !exit; exit = act())
		{ }
	}

	public bool OpenHub()
	{
		Console.Clear();
		ConsoleUtil.PrintTextFile("Starfall.Resources.pages.Hub.txt");

		var select = MenuUtil.OpenMenu(
			"[#d1949e]1[/]. 상태 보기", "[#d1949e]2[/]. 인벤토리",
			"[#d1949e]2[/]. 퀘스트 보기", "[#d1949e]3[/]. 상점",
			"[#d1949e]4[/]. 던전입장", "[#d1949e]5[/]. 휴식하기",
			"[#d1949e]6[/]. 저장하기", "[#d1949e]7[/]. 메인으로",
			"[#d1949e]8[/]. 게임 종료");

		act = select switch
		{
			// 상태보기
			0 => OpenStatus,
			// 인벤토리
			1 => OpenInventory,
			// 퀘스트 보기
			2 => () =>
			{
				QuestManager.EnterQuestMenu(player);
				return true;
			}
			,
			// 상점
			3 => OpenShop,
			// 던전입장
			4 => JoinDungeon,
			// 휴식하기
			5 => () => Rest(500),
			// 저장하기
			6 => Save,
			// 메인으로
			7 => () =>
			{
				GameManager.EnterMain();
				return true;
			}
			,
			_ => act
		};

		// 게임 종료 선택했을 때 게임 종료
		return select == 8;
	}

	private bool OpenStatus()
	{
		string StatView(float val) => val != 0 ?
			"([#f5b83d]" + (val > 0 ? "+" : "-") + $"[/][#d1949e]{Math.Abs(val)}[/]" + ")"
		: "";

		Console.Clear();
		AnsiConsole.MarkupLine($"""
      상태보기
      캐릭터의 정보가 표시됩니다.
      Lv. [#d1949e]{player.level:D2}[/]
      {player.name} ( {player.job.GetJobNameToKor()} )
      공격력 : [#d1949e]{player.TrueAtk}[/] {StatView(player.GetAddtionalAtk())}
      방어력 : [#d1949e]{player.TrueDef}[/] {StatView(player.GetAddtionalDef())}
      체 력 : [#d1949e]{player.TrueHp}[/] {StatView(player.GetAddtionalHp())}
      마 력 : [#d1949e]{player.TrueMp}[/] {StatView(player.GetAddtionalMp())}
      Gold : [#d1949e]{player.gold}[/] G
      """);

		Console.WriteLine();
		MenuUtil.OpenMenu("[#d1949e]0[/]. 나가기");
		act = OpenHub;
		return false;
	}

	private bool OpenInventory()
	{
		Console.Clear();
		AnsiConsole.MarkupLine("""
      인벤토리
      보유 중인 아이템을 관리할 수 있습니다.

      [[아이템 목록]]
      """);

		foreach (var (item, equip) in player.inventory)
		{
			var option = new StringBuilder();


			option.Append($"- {item.Type.GetName()} / {(equip == 1 ? "[[E]]" : "")}{item.Name} | ");
			var stats = new List<string>();
			if (item.Atk != 0) stats.Add("공격력 " + (item.Atk > 0 ? "+" + item.Atk : item.Atk));
			if (item.Def != 0) stats.Add("방어력 " + (item.Def > 0 ? "+" + item.Def : item.Def));
			if (item.Hp != 0) stats.Add("생명력 " + (item.Hp > 0 ? "+" + item.Hp : item.Hp));
			if (item.Mp != 0) stats.Add("정신력 " + (item.Mp > 0 ? "+" + item.Mp : item.Mp));
			option.Append(string.Join(" / ", stats));
			option.Append(" | " + item.Description);
			if (item.Type == ItemType.Consumable && player.inventory.ContainsKey(item))
			{
				option.Append($" x{(Math.Abs(player.inventory[item]))}");
			}

			AnsiConsole.MarkupLine(option.ToString());
		}

		switch (MenuUtil.OpenMenu("\n장착 관리 ", "나가기"))
		{
			case 0:
				// Edit equip
				act = EditInventory;
				return false;

			// 나가기
			case 1:
			default:
				act = OpenHub;
				return false;
		}
	}

	// 아이템 장착 메뉴 이전 선택값 저장
	private int savedIndex = 0;
	private bool EditInventory()
	{
		Console.Clear();
		AnsiConsole.MarkupLine("""
      인벤토리 - 장착 관리
      보유 중인 아이템을 관리할 수 있습니다.

      [[아이템 목록]]
      """);
		var menu = new List<string>();
		var items = new List<Item>();
		int index = 0;
		foreach (var (item, equip) in player.inventory)
		{
			var option = new StringBuilder();
			option.Append($"- {index + 1} / {item.Type.GetName()} / {(equip == 1 ? "[[E]]" : "")}{item.Name} | ");
			var stats = new List<string>();
			if (item.Atk != 0) stats.Add("공격력 " + (item.Atk > 0 ? "+" : "") + item.Atk);
			if (item.Def != 0) stats.Add("방어력 " + (item.Def > 0 ? "+" : "") + item.Def);
			if (item.Hp != 0) stats.Add("생명력 " + (item.Hp > 0 ? "+" : "") + item.Hp);
			if (item.Mp != 0) stats.Add("정신력 " + (item.Mp > 0 ? "+" : "") + item.Mp);
			option.Append(string.Join(" / ", stats));
			option.Append(" | " + item.Description);
			if (item.Type == ItemType.Consumable && player.inventory.ContainsKey(item))
			{
				option.Append($" x{(Math.Abs(player.inventory[item]))}");
			}
			items.Add(item);
			index++;
			menu.Add(option.ToString());
		}

		this.savedIndex = MenuUtil.OpenMenu(this.savedIndex, false, [.. menu, "\n0. 나가기"]);

		switch (this.savedIndex)
		{
			case var i when i > -1 && i < index:
				var item = items[i];

				if (item.Type == ItemType.Consumable)
				{
					bool isHpPotion = item.Hp > 0;
					float recoveryAmount = isHpPotion ? item.Hp : item.Mp;
					bool isFull = isHpPotion ? player.hp >= 100 : player.mp >= 50;

					if (isFull)
					{
						AnsiConsole.MarkupLine($"\n{(isHpPotion ? "체력이" : "마나가")} 이미 최대치입니다. {item.Name}을 사용할 수 없습니다.");

						Thread.Sleep(1500);

						return false;
					}

					if (isHpPotion)
					{
						player.hp = Math.Min(player.hp + recoveryAmount, 100);
					}
					else
					{
						player.mp = Math.Min(player.mp + recoveryAmount, 50);
					}

					AnsiConsole.MarkupLine($"\n {item.Name}을 사용하여 {(isHpPotion ? "Hp" : "Mp")}를 {recoveryAmount}만큼 회복했습니다.\n");

					if (player.inventory[item] == 0)
					{
						player.inventory.Remove(item);
					}
					else
					{
						player.inventory[item] += 1;
					}

						MenuUtil.OpenMenu("확인");
				}
				else
				{
					if ((player.inventory[item] = -player.inventory[item]) == 1)
					{
						var type = item.Type;
						foreach (var (target, equip) in player.inventory)
						{
							if (equip == 1 && target != item
							&& target.Type == type)
							{
								player.inventory[target] = -1;
							}
						}
					}
				}
				act = EditInventory;
				return false;

			default:
				act = OpenInventory;
				this.savedIndex = 0;
				return false;
		}
	}

	public bool OpenShop()
	{
		// 뒤로가기 선택되면 해당 메서드 빠져나옴
		Shop.EnterShop(shop, player);
		act = OpenHub;
		return false;
	}

	public List<DungeonData> dungeons =
	[
		new ("쉬운 던전", 5, 40, 1000),
			new ("일반 던전", 11, 40, 1700),
			new ("어려운 던전", 17, 40, 2500),
		];
	public bool JoinDungeon()
	{
		new Floor().Render();
		// Console.Clear();
		// AnsiConsole.MarkupLine("""
		//   던전입장
		//   이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.

		//   """);

		// var menu = new List<string>();
		// var index = 0;
		// foreach (var (label, def, _, _) in dungeons)
		// {
		// 	menu.Add($"{++index}. {label} | 방어력 {Math.Round(def)} 이상 권장");
		// }

		// var select = MenuUtil.OpenMenu([.. menu, "0. 나가기"]);
		// if (select > -1 && select < index)
		// {
		// 	Console.Clear();
		// 	var (label, def, fail, dReward) = dungeons[select];

		//         var battle = new Battle();
		//         battle.StartBattle(player, GameManager.monsters);
		//     }

		// // 던전이 끝났을시 허브로
		act = OpenHub;
		return false;
	}

	public bool Rest(int requireGold)
	{
		Console.Clear();
		AnsiConsole.MarkupLine($"""
      휴식하기
      {requireGold} G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.gold} G)
      
      """);
		if (MenuUtil.OpenMenu("휴식하기", "나가기") == 0)
		{
			Console.Clear();
			if (player.gold >= requireGold)
			{
				player.gold -= requireGold;
				player.hp = 100;
				AnsiConsole.MarkupLine("휴식을 완료했습니다.\n");

				MenuUtil.OpenMenu("확인");
			}
			else
			{
				AnsiConsole.MarkupLine("Gold 가 부족합니다.\n");

				MenuUtil.OpenMenu("확인");
			}
		}

		act = OpenHub;
		return false;
	}

	public bool Save()
	{
		GameData data = player;
		StorageController.SaveBinary("data", data);
		act = OpenHub;
		return false;
	}
}
