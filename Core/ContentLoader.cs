using Spectre.Console;
using Starfall.Contents;
using Starfall.PlayerService;

namespace Starfall.Core;

public static class ContentLoader
{
  private static readonly Random random = new();
  public static void Load()
  {
    RegisterSkill(
      "단단해지기",
      "마나 10을 소모하여 1턴동안 방어력이 3 증가합니다.",
      10,
      1,
      0,
      new()
      {
        ["Use"] = (player, _) => player.def += 3,
        ["Off"] = (player, _) => player.def -= 3
      }
    );

    RegisterSkill(
      "알파 스트라이크",
      "마나 10을 소모하여 1명의 적에게 피해를 {0} * 2 만큼 줍니다.",
      10,
      0,
      1,
      new()
      {
        ["Use"] = (player, list) => list[0].hp -= 2 * player.TrueAtk,
      }
    );

    RegisterSkill(
      "자가 회복",
      "마나 20을 소모하여 체력을 {1} * 2 회복합니다.",
      20,
      0,
      0,
      new()
      {
        ["Use"] = (player, list) => player.hp += 2 * player.def,
      }
    );

    RegisterSkill(
      "더블 스트라이크",
      "마나 15을 소모하여 무작위 2명의 적에게 피해를 {0} * 1.5 만큼 줍니다.",
      15,
      0,
      100,
      new()
      {
        ["Use"] = (player, list) =>
        {
          var target = (from mob in list
                        orderby random.Next()
                        select mob).Take(2);
          foreach (var mob in target)
          {
            mob.hp -= 1.5f * player.TrueAtk;
          }
        },
      }
    );

    RegisterEvent(
      "이벤트1",
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
  }

  private static void RegisterSkill(string name, string desc, int cost, int durationTurn, int targetAmount, Dictionary<string, SkillAction> actions)
    => GameManager.skills.Add(name, new(name, desc, cost, durationTurn, targetAmount, actions));

  private static void RegisterEvent(string name, string desc, Dictionary<string, Action<Player, List<Monster>>> actions)
    => GameManager.events.Add(name, new(name, desc, actions));
}