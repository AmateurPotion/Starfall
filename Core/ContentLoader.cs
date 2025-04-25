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
        ["Use"] = (player, list) => list[0].Hp -= 2 * player.TrueAtk,
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
            mob.Hp -= 1.5f * player.TrueAtk;
          }
        },
      }
    );
    
    RegisterEvent(
      "이벤트1",
      "세 가지 중 하나를 선택하세요.",
      new()
      {
        ["선택지1"] = () => AnsiConsole.MarkupLine("선택지1 실행됨"),
        ["선택지2"] = () => AnsiConsole.MarkupLine("선택지2 실행됨"),
        ["선택지3"] = () => AnsiConsole.MarkupLine("선택지3 실행됨")
      }
    );
  }

  private static void RegisterSkill(string name, string desc, int cost, int durationTurn, int targetAmount, Dictionary<string, SkillAction> actions)
    => GameManager.skills.Add(name, new(name, desc, cost, durationTurn, targetAmount, actions));

  private static void RegisterEvent(string name, string desc, Dictionary<string, Action> actions)
    => GameManager.events.Add(new(name, desc, actions));
}