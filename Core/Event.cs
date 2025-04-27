using Spectre.Console;
using Starfall.IO.CUI;
using Starfall.PlayerService;

namespace Starfall.Contents;


public class Event(string name, string description, (string key, Action<Player, Dictionary<string, Action<Player, Monster[]>>>)[] actions)
{
  // 이벤트 이름
  public string name = name;
  // 이벤트 설명
  public string description = description;
  public Dictionary<string, Action<Player, Monster[]>> events = [];
  private readonly (string key, Action<Player, Dictionary<string, Action<Player, Monster[]>>> action)[] actions = actions;

  public void Action(Player player)
  {
  Render:
    Console.Clear();
    AnsiConsole.MarkupLine($"""
    [[{name}]]
    {description}

    선택지를 골라주세요: 

    """);

    var selected = MenuUtil.OpenMenu([.. (from pair in actions select pair.key)]);

    // var selected = AnsiConsole.Prompt(
    //   new SelectionPrompt<string>()
    //   .Title("선택지를 골라주세요:")
    //   .AddChoices(actions.Keys)
    // );

    AnsiConsole.MarkupLine($"{selected} 을(를) 선택했습니다.");

    if (selected > -1 && selected < actions.Length)
      actions[selected].action(player, events);
    else goto Render;
  }
}