using Spectre.Console;
using Starfall.Contents;
using Starfall.Contents.Json;
using Starfall.PlayerService;

namespace Starfall.Core;

public static class ContentLoader
{
	private static readonly Random random = new();
	public static void Load()
	{

		#region Skill
		RegisterSkill(
			"단단해지기",
			"마나 10을 소모하여 1턴동안 방어력이 3 증가합니다.",
			10,
			1,
			0,
			new()
			{
				["Use"] = (player, _) =>
				{
					player.def += 3;
					AnsiConsole.MarkupLine("방어력이 3 증가했습니다!");
				},
				["Off"] = (player, _) =>
				{
					player.def -= 3;
					AnsiConsole.MarkupLine("방어력 버프의 지속시간이 끝났습니다.");
				}
			}
		);

		RegisterSkill(
			"알파 스트라이크",
			"마나 10을 소모하여 1명의 적에게 공격력의 200%만큼 피해를 줍니다.",
			10,
			0,
			1,
			new()
			{
				["Use"] = (player, list) =>
				{
					Battle.PlayerAttackMonster(player, list[0], 200);
				},
			}
		);

		RegisterSkill(
			"자가 회복",
			"마나 20을 소모하여 체력을 방어력의 200%만큼 회복합니다.",
			20,
			0,
			0,
			new()
			{
				["Use"] = (player, list) =>
				{
					player.hp += 2 * player.def;
					AnsiConsole.MarkupLine($"{player.def}의 체력을 회복했습니다!");
				},
			}
		);

		RegisterSkill(
			"더블 스트라이크",
			"마나 15을 소모하여 무작위 2명의 적에게 공격력의 150%만큼 피해를 줍니다.",
			15,
			0,
			100,
			new()
			{
				["Use"] = (player, list) =>
				{
					/*var target = (from mob in list
												orderby random.Next()
												select mob).Take(2);
					foreach (var mob in target)
					{
						Battle.PlayerAttackMonster(player, mob, 150);
					}*/
					for (int i = 0; i < 2; i++)
					{
						Battle.PlayerAttackMonster(player, list[random.Next(list.Count)], 150);
					}
				},
			}
		);

		#endregion

		#region Events
		RegisterEvent(
			"이상한 부족의 환영",
			"플레이어는 원주민을 만났습니다. 원주민 중 한 명을 골라주세요.",
			new()
			{
				["트랄랄레로 트랄랄라"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("트랄랄레로 트랄랄라가 플레이어의 체력을회복");
					player.hp += 100;
				},
				["봄바르딜로 코르코딜로"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("봄바르딜로 코르코딜로가 몬스터들을 공격");
					foreach (var m in list)
					{
						m.hp -= 100;
					}
				},
				["퉁x9 사후르"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("퉁x9 사후르가 승리의 함성을 외침");
					player.atk += 10;
				}
			}
		);

		RegisterEvent(
			"세 개의 상자",
			"플레이어는 던전을 탐험하다 3개의 상자를 발견했습니다. 하나만 열 수 있을 것 같습니다.",
			new()
			{
				["상자 1"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("상자 1에는 함정이 있었습니다. ");
					player.hp -= 20;
				},
				["상자 2"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("상자 2에서 약간의 금화를 발견했습니다.");
					player.gold += 500;
				},
				["상자 3"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("상자 3에는 많은 금화가 있었습니다. 보물을 챙기다 상처를 입었습니다.");
					player.gold += 1000;
					player.hp -= 10;
				}
			}
		);

		RegisterEvent(
			"숨겨진 쉼터",
			"플레이어는 던전을 탐험하다 비밀장소를 발견했습니다. 그 장소에서...",
			new()
			{
				["휴식"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("500G를 통해 안전을 보장받을 수 있었습니다.");
					player.gold -= 500;
					player.hp += 100;
				},
				["아이템"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("약간의 골드를 획득했습니다.");
					player.gold += 500;
				},
				["싸움"] = (player, list) =>
				{
					AnsiConsole.MarkupLine("먼저 와 있던 몬스터가 존재했습니다.");
					player.hp -= 20;
				}
			}
		);
		#endregion

		#region Dungeon
		var floors = LoadFloors("늪지", "끔찍한늪지", "평야", "해골무덤");
		RegisterDungeon(new("쉬운 던전", floors)
		{
			requireAtk = 1,
			requireDef = 5
		});

		RegisterDungeon(new("일반 던전", floors)
		{
			requireAtk = 1,
			requireDef = 11
		});

		RegisterDungeon(new("어려운 던전", floors)
		{
			requireAtk = 1,
			requireDef = 17
		});
		#endregion
	}

	private static void RegisterSkill(string name, string desc, int cost, int durationTurn, int targetAmount, Dictionary<string, SkillAction> actions)
		=> GameManager.skills.Add(name, new(name, desc, cost, durationTurn, targetAmount, actions));

	private static void RegisterEvent(string name, string desc, Dictionary<string, Action<Player, List<Monster>>> actions)
		=> GameManager.events.Add(name, new(name, desc, actions));

	private static FloorData[] LoadFloors(params string[] names)
		=> [.. from name in names select GameManager.floors[name]];
	private static void RegisterDungeon(Dungeon dungeon)
		=> GameManager.dungeons.Add(dungeon.label, dungeon);

}
