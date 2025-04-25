using Spectre.Console;
using Starfall.PlayerService;

namespace Starfall.Contents;

//public delegate void EventAction(Player player, List<Monster> list);
public delegate void EventTrigger();

public class Event(string name, string description/*, EventAction actions*/, List<string> options, EventTrigger? onFinish = null)
{
  // 이벤트 이름
  public string name = name;
  // 이벤트 설명
  public string description = description;
  //public EventAction actions = actions;
  public List<string> options = options;
  public EventTrigger? onFinish = onFinish;

  public void Action(Player player, List<Monster> list)
  {
    AnsiConsole.MarkupLine($"{name}\n{description}\n");

    var choices = new List<string> {"선택지1", "선택지2", "선택지3"};
    var selected = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("선택지를 골라주세요:")
      .AddChoices(choices)
    );

    onFinish?.Invoke();
  }
}