using System.Collections;
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
public class Shop : IEnumerable<Item>
{
	public List<Item> sellItems = [];
	public float sellRatio = 0.85f;

	public Shop(params string[] itemNames)
	{
		var itemDic = GameManager.items;

		foreach (var key in itemNames)
		{
			if (itemDic.TryGetValue(key, out var value))
			{
				sellItems.Add(value);
			}
		}
	}

	public Item this[int i]
	{
		get => sellItems[i];
		set => sellItems[i] = value;
	}

	public IEnumerator<Item> GetEnumerator()
		=> sellItems.GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator()
		=> GetEnumerator();

	private static string StatStr(float value) => (value > 0 ? "+" : "-") + Math.Abs(value);

	public static void EnterShop(Shop shop, Player player)
	{
	RestartShop:
		Console.Clear();
		AnsiConsole.MarkupLine($"""
				상점
				아이템을 구매 / 판매할 수 있는 상점입니다.
				[[보유 골드]]
				{player.gold} G
				[[아이템 목록]]
				""");

		var menu = new List<string>();
		var index = 0;
		foreach (var item in shop)
		{
			var option = new StringBuilder($"- {++index} ");

			option.Append($"{item.Type.GetName()} / {item.Name} |");
			var stats = new List<string>();
			if (item.Atk != 0) stats.Add("공격력 " + StatStr(item.Atk));
			if (item.Def != 0) stats.Add("방어력 " + StatStr(item.Def));
			if (item.Hp != 0) stats.Add("생명력 " + StatStr(item.Hp));
			if (item.Mp != 0) stats.Add("정신력 " + StatStr(item.Mp));
			option.Append(string.Join(" / ", stats));
			option.Append($" | {item.Description} | ");
			//소모템일 경우 보유 개수를 보여줌
			if (item.Type == ItemType.Consumable)
			{
				var count = player.inventory.TryGetValue(item, out var val) ? Math.Abs(val) : 0;
				option.Append($"{count}개 보유 / {item.Price} G");
			}
			//소모템이 아닐 경우(= 장비템) 구매여부를 보여줌
			else
			{
				option.Append(player.inventory.ContainsKey(item) ? "구매완료" : item.Price + " G");
			}

			menu.Add(option.ToString());
		}

		int returnIndex = MenuUtil.OpenMenu(0, false, [.. menu, "\n0. 나가기"]);

		if (returnIndex == menu.Count)
		{
			return;
		}

		SelectItemOnShop(shop, shop.sellItems[returnIndex], player);
		goto RestartShop;
	}


	public static void SelectItemOnShop(Shop shop, Item item, Player player)
	{
		Console.Clear();
		var equip = "";
		if (player.inventory.TryGetValue(item, out var e) && e == 1)
		{
			equip = "[[E]]";
		}
		AnsiConsole.MarkupLine($"""
				{equip} {item.Type.GetName()} - {item.Name}
				{item.Description}
				공격력 {StatStr(item.Atk)}
				방어력 {StatStr(item.Def)}
				생명력 {StatStr(item.Hp)}
				정신력 {StatStr(item.Mp)}

				""");
		if (item.Type == ItemType.Consumable)
		{
			if (player.inventory.ContainsKey(item))
			{
				var sellPrice = (int)Math.Round(shop.sellRatio * item.Price);
				int selection = MenuUtil.OpenMenu(
					$"구매하기 / {item.Price} G {(player.gold < item.Price ? "(골드부족)" : "")}",
					$"판매하기 / {sellPrice} G",
					"뒤로가기"
				);

				if (selection == 0) // 구매
				{
					if (player.gold >= item.Price)
					{
						player.gold -= item.Price;
						player.inventory[item] -= 1;
						AnsiConsole.MarkupLine($"\n{item.Name}을(를) 1개 구매했습니다.");
					}
					else
					{
						AnsiConsole.MarkupLine("\nGold 가 부족합니다.");
					}
				}
				//판매
				else if (selection == 1)
				{
					player.gold += sellPrice;
					player.inventory[item] += 1;
					if (player.inventory[item] == 0)
					{
						player.inventory.Remove(item);
					}
					AnsiConsole.MarkupLine($"\n{sellPrice} G에 {item.Name}을 판매했습니다.");
				}
				else if (selection == 2)
				{
					return;
				}
			}
			// 처음 구매
			else
			{
				if (player.gold >= item.Price)
				{
					player.gold -= item.Price;
					player.inventory.Add(item, -1);
					AnsiConsole.MarkupLine($"\n{item.Name}을(를) 1개 구매했습니다.");
				}
				else
				{
					AnsiConsole.MarkupLine("\nGold 가 부족합니다.");
				}
			}

			MenuUtil.OpenMenu("확인");
			return;
		}
		else
		{
			// 장비아이템 로직
			if (player.inventory.ContainsKey(item))
			{
				// 아이템이 있을 시 판매
				var sellPrice = (int)Math.Round(shop.sellRatio * item.Price);
				if (MenuUtil.OpenMenu($"판매하기 / {sellPrice} G", "뒤로가기") == 0)
				{
					player.gold += sellPrice;
					player.inventory.Remove(item);
					AnsiConsole.MarkupLine($"\n{sellPrice} G에 {item.Name}을 판매했습니다.");
					MenuUtil.OpenMenu("확인");
				}
			}
			else
			{
				// 아이템이 없을 시 구매
				if (MenuUtil.OpenMenu($"구매하기 / {item.Price} G {(player.gold < item.Price ? "(골드부족)" : "")}", "뒤로가기") == 0)
				{
					if (player.gold >= item.Price)
					{
						player.gold -= item.Price;
						player.inventory.Add(item, -1);
						AnsiConsole.MarkupLine("\n구매를 완료했습니다.");
					}
					else
					{
						AnsiConsole.MarkupLine("\nGold 가 부족합니다.");
					}

					MenuUtil.OpenMenu("확인");
				}
			}
		}
	}
}
