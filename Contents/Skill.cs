using Spectre.Console;
using Starfall.IO.CUI;
using Starfall.PlayerService;

namespace Starfall.Contents;

public delegate int SkillAction(Player player, Monster[] list, int turn);

public class Skill(string name, string description, int cost, int durationTurn, int targetAmount, Dictionary<string, SkillAction> actions)
{
  // 스킬 이름
  public string name = name;
  // 스킬 설명
  public string description = description;
  // 스킬 요구 마나
  public int cost = cost;
  // 0일시 자신에게 사용하는 스킬
  // 1 ~ {적 수}보다 적을 시 타겟을 지정하여 사용하는 스킬
  // {적 수} 이상일시 전체 혹은 무작위로 사용하는 스킬
  public int targetAmount = targetAmount;
  // 0 이면 즉발스킬
  // 1 이상이면 버프 스킬
  public int durationTurn = durationTurn;
  public Dictionary<string, SkillAction> actions = actions;
  public int Action(string eventName, Player player, Monster[] list, int turn)
  {
    if (eventName != "Use")
    {
      return 0;
    }

    AnsiConsole.MarkupLine(description);
    MenuUtil.OpenMenu("다음");

    if (actions.TryGetValue(eventName, out var action)) return action(player, list, turn);

    return 0;
  }
}