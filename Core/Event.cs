using Spectre.Console;
using Starfall.PlayerService;

namespace Starfall.Contents;

public delegate void EventAction(string option, Action action);

public class Event(string name, string description, Dictionary<string, Action<Player, List<Monster>>> actions)
{
  // 이벤트 이름
  public string name = name;
  // 이벤트 설명
  public string description = description;
  public Dictionary<string, Action<Player, List<Monster>>> actions = actions;

  public void Action(Player player, List<Monster> list, Action onFinish)
  {
    AnsiConsole.MarkupLine($"[[{name}]]\n{description}\n");

    var selected = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("선택지를 골라주세요:")
      .AddChoices(actions.Keys)
    );

    AnsiConsole.MarkupLine($"{selected} 을(를) 선택했습니다.");

    if (actions.TryGetValue(selected, out var onSelect))
    {
      onSelect?.Invoke(player, list);
    }

    onFinish?.Invoke();
  }
}