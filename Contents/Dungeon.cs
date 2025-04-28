using Spectre.Console;
using Starfall.Contents.Json;
using Starfall.IO.CUI;
using Starfall.PlayerService;

namespace Starfall.Contents;

public class Dungeon
{
  private static Player Player = GameManager.Game!.player;
  private static Dictionary<string, Item> Items => GameManager.items;
  private static Dictionary<string, Event> Events => GameManager.events;
  private static Dictionary<string, MonsterData> Monsters => GameManager.monsters;
  public float requireDef = 0;
  public float requireAtk = 0;
  public string label = "";
  public Floor[] floors;

  #region StartInfo
  public float startHp = 0, startGold = 0;
  #endregion

  public Dungeon(string label, FloorData[] floorDatas)
  {
    this.label = label;
    floors = new Floor[floorDatas.Length];

    for (int i = 0; i < floorDatas.Length; i++)
    {
      var data = floorDatas[i];

      floors[i] = new(
        [.. (from key in data.ItemPool where Items.ContainsKey(key) select Items[key])],
        [.. (from key in data.MobPool where Monsters.ContainsKey(key) select Monsters[key])],
        [.. (from key in data.EventPool where Events.ContainsKey(key) select Events[key])],
         i + 1, data.Length, Math.Max(data.Width, 5));
    }
  }

  public void Join()
  {
    startHp = Player.TrueHp;
    startGold = Player.gold;

    for (int i = 0; i < floors.Length; i++)
    {
      var floor = floors[i];

      if (floor.Render())
      {
        // 층을 클리어했을시
        if (i < floors.Length - 1)
        {
        Render:
          Console.Clear();
          AnsiConsole.MarkupLine($"""
          축하합니다! 던전 {floor.index}층을 클리어하셨습니다.
          다음 층으로 내려가시거나 탈출하시겠습니까?
          
          """);

          switch (MenuUtil.OpenMenu("올라가기", "탈출!"))
          {
            case 0: continue;
            case 1: return;
            default: goto Render;
          }
        }
        else
        {
          // 던전 클리어
          Console.Clear();
          AnsiConsole.MarkupLine($"""
          던전 클리어
          축하합니다!!
          {label}을 클리어 하였습니다.

          [탐험 결과]
          체력 {startHp} -> {Player.TrueHp}
          Gold {startGold} G -> {Player.gold} G
          
          """);

          MenuUtil.OpenMenu("0. 나가기");
          return;
        }
      }
      else
      {
        // 패배(사망시)
      }
    }
  }
}