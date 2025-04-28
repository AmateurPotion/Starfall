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
			"마나를 10 소모하여 1턴동안 방어력이 3 증가합니다.",
			10,
			1,
			0,
			new()
			{
				["Use"] = (player, _, _) =>
				{
					player.def += 3;
					AnsiConsole.MarkupLine("방어력이 3 증가했습니다!");
					return 0;
				},
				["End"] = (player, _, turn) =>
				{
					player.def -= 3;
					AnsiConsole.MarkupLine("방어력 버프의 지속시간이 끝났습니다.");
					return turn - 1;
				}
			}
		);

		RegisterSkill(
			"분노",
			"마나를 20 소모하여 2턴동안 공격력이 5 증가합니다.",
			20,
			2,
			0,
			new()
			{
				["Use"] = (player, _, _) =>
				{
					player.def += 3;
					AnsiConsole.MarkupLine("방어력이 3 증가했습니다!");
					return 0;
				},
				["End"] = (player, _, turn) =>
				{
					player.def -= 3;
					AnsiConsole.MarkupLine("방어력 버프의 지속시간이 끝났습니다.");
					return turn - 1;
				}
			}
		);

		RegisterSkill(
			"알파 스트라이크",
			"마나를 10 소모하여 1명의 적에게 공격력의 200%만큼 피해를 줍니다.",
			10,
			0,
			1,
			new()
			{
				["Use"] = (player, list, _) =>
				{
					Battle.PlayerAttackMonster(player, list[0], 200);
					return 0;
				},
			}
		);

		RegisterSkill(
			"베타 스트라이크",
			"마나를 25 소모하여 1명의 적에게 공격력의 300%만큼 피해를 줍니다.",
			25,
			0,
			1,
			new()
			{
				["Use"] = (player, list, _) =>
				{
					Battle.PlayerAttackMonster(player, list[0], 300);
					return 0;
				},
			}
		);

		RegisterSkill(
			"더블 스트라이크",
			"마나를 15 소모하여 무작위 2명의 적에게 공격력의 150%만큼 피해를 줍니다.",
			15,
			0,
			100,
			new()
			{
				["Use"] = (player, list, _) =>
				{
					for (int i = 0; i < 2; i++)
					{
						Battle.PlayerAttackMonster(player, list[random.Next(list.Length)], 150);
					}

					return 0;
				},
			}
		);

		RegisterSkill(
			"트리플 스트라이크",
			"마나를 25 소모하여 무작위 3명의 적에게 공격력의 100%만큼 피해를 줍니다.",
			25,
			0,
			100,
			new()
			{
				["Use"] = (player, list, _) =>
				{
					for (int i = 0; i < 3; i++)
					{
						Battle.PlayerAttackMonster(player, list[random.Next(list.Length)], 150);
					}
					return 0;
				},
			}
		);

		RegisterSkill(
			"메테오 스트라이크",
			"마나를 100 소모하여 모든 적에게 공격력의 500%만큼 피해를 줍니다.",
			100,
			0,
			100,
			new()
			{
				["Use"] = (player, list, _) =>
				{
					foreach (Monster m in list)
					{
						Battle.PlayerAttackMonster(player, m, 500);
					}
					return 0;
				},
			}
		);

		RegisterSkill(
			"파이어볼",
			"마나를 30 소모하여 1명의 적에게 공격력의 400%만큼 피해를 줍니다.",
			30,
			0,
			1,
			new()
			{
				["Use"] = (player, list, _) =>
				{
					Battle.PlayerAttackMonster(player, list[0], 400);
					return 0;
				},
			}
		);

		RegisterSkill(
			"자가 회복",
			"마나를 20 소모하여 체력을 방어력의 200%만큼 회복합니다.",
			20,
			0,
			0,
			new()
			{
				["Use"] = (player, list, _) =>
				{
					player.hp += 2 * player.def;
					AnsiConsole.MarkupLine($"{player.def}의 체력을 회복했습니다!");
					return 0;
				},
			}
		);

		RegisterSkill(
			"대량 회복",
			"마나를 50 소모하여 체력을 방어력의 300%만큼 회복합니다.",
			20,
			0,
			0,
			new()
			{
				["Use"] = (player, list, _) =>
				{
					player.hp += 3 * player.def;
					AnsiConsole.MarkupLine($"{player.def}의 체력을 회복했습니다!");
					return 0;
				},
			}
		);
		#endregion

		#region Events
		RegisterEvent(
			"이상한 부족의 환영",
			"플레이어는 원주민을 만났습니다. 원주민 중 한 명을 골라주세요.",
				("트랄랄레로 트랄랄라", (player, _) =>
				{
					AnsiConsole.MarkupLine("트랄랄레로 트랄랄라가 플레이어의 체력을회복");
					player.hp += 100;
				}
		),
				("봄바르딜로 코르코딜로", (player, events) =>
				{
					AnsiConsole.MarkupLine("봄바르딜로 코르코딜로가 몬스터들을 공격(다음 전투에서 몬스터들이 피해를 입습니다.)");
					events["NextBattle"] = (_, mobs) =>
					{
						foreach (var m in mobs)
						{
							m.hp -= 100;
						}
					};
				}
		),
				("퉁x9 사후르", (player, _) =>
				{
					AnsiConsole.MarkupLine("퉁x9 사후르가 승리의 함성을 외침");
					player.atk += 10;
				}
		)
		);

		RegisterEvent(
			"세 개의 상자",
			"플레이어는 던전을 탐험하다 3개의 상자를 발견했습니다. 하나만 열 수 있을 것 같습니다.",
				("상자 1", (player, _) =>
				{
					AnsiConsole.MarkupLine("상자 1에는 함정이 있었습니다. ");
					player.hp -= 20;
				}
		),
				("상자 2", (player, _) =>
				{
					AnsiConsole.MarkupLine("상자 2에서 약간의 금화를 발견했습니다.");
					player.gold += 500;
				}
		),
				("상자 3", (player, _) =>
				{
					AnsiConsole.MarkupLine("상자 3에는 많은 금화가 있었습니다. 보물을 챙기다 상처를 입었습니다.");
					player.gold += 1000;
					player.hp -= 10;
				}
		)
		);

		RegisterEvent(
			"숨겨진 쉼터",
			"플레이어는 던전을 탐험하다 비밀장소를 발견했습니다. 그 장소에서...",
				("휴식", (player, _) =>
				{
					AnsiConsole.MarkupLine("500G를 통해 안전을 보장받을 수 있었습니다.");
					player.gold -= 500;
					player.hp += 100;
				}
		),
				("아이템", (player, _) =>
				{
					AnsiConsole.MarkupLine("약간의 골드를 획득했습니다.");
					player.gold += 500;
				}
		),
				("싸움", (player, _) =>
				{
					AnsiConsole.MarkupLine("먼저 와 있던 몬스터가 존재했습니다.");
					player.hp -= 20;
				}
		)
		);
		#endregion

		#region Dungeon
		RegisterDungeon(new("쉬운 던전", LoadFloors("평야"))
		{
			requireAtk = 1,
			requireDef = 5
		});

		RegisterDungeon(new("일반 던전", LoadFloors("늪지", "끔찍한늪지"))
		{
			requireAtk = 1,
			requireDef = 11
		});

		RegisterDungeon(new("어려운 던전", LoadFloors("늪지", "끔찍한늪지", "평야", "해골무덤"))
		{
			requireAtk = 1,
			requireDef = 17
		});
		#endregion
	}

	private static void RegisterSkill(string name, string desc, int cost, int durationTurn, int targetAmount, Dictionary<string, SkillAction> actions)
		=> GameManager.skills.Add(name, new(name, desc, cost, durationTurn, targetAmount, actions));


	private static void RegisterEvent(string name, string desc, params (string key, Action<Player, Dictionary<string, Action<Player, Monster[]>>>)[] actions)
		=> GameManager.events.Add(name, new(name, desc, actions));

	private static FloorData[] LoadFloors(params string[] names)
		=> [.. from name in names select GameManager.floors[name]];
	private static void RegisterDungeon(Dungeon dungeon)
		=> GameManager.dungeons.Add(dungeon.label, dungeon);

}
