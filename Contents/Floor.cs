using Spectre.Console;
using Starfall.Contents.Json;
using Starfall.Core;
using Starfall.PlayerService;
using Starfall.Utils;

namespace Starfall.Contents;

public class Floor
{
  private static readonly Random random = new();
  private static Player Player => GameManager.Game!.player;
  public readonly int length;
  public readonly Item[] itemPool;
  public readonly MonsterData[] monsterPool;
  public readonly Event[] eventPool;
  public List<StageNode>[] data;
  public Vector2Int current = new(-1, -1);
  public int Width => data.Length;
  public int index;
  public Floor(Item[] items, MonsterData[] monsters, Event[] events, int index, int length = 5, int width = 5)
  {
    this.index = index;
    itemPool = items;
    eventPool = events;
    monsterPool = monsters;
    this.length = length + 3;
    data = new List<StageNode>[width];

    #region GenerateFloorMap
    // TODO 가장 위 라인, 가장 아래 노드 라인일 경우 위나 아래로 못 이동하게 구현
    StageNode bossNode = new() { type = StageType.Boss },
    lastShopNode = new() { type = StageType.Shop };

    for (int y = 0; y < data.Length; y++)
    {
      // 라인 생성
      var line = data[y] = [];

      // 기초 스테이지 생성
      for (int x = 0, l = this.length; x < l; x++)
      {
        line.Add(x switch
        {
          // 시작 노드
          0 => new() { type = StageType.Start },
          // 보스 노드
          var i when i == l - 2 => lastShopNode,
          var i when i == l - 1 => bossNode,
          // 기본 전투 생성
          _ => new() { type = StageType.Battle }
        });
      }
    }

    // 각 라인마다 2개 노드 삭제하기
    for (int x = 0; x < length; x++)
    {
      var removeList = (from line in data
                        where line[x + 1].type != StageType.None
                        orderby random.Next()
                        select line[x + 1]).Take(2);

      foreach (var node in removeList)
      {
        node.type = StageType.None;
      }
    }

    //  이벤트 스테이지 삽입
    int eventAmount = length < 3 ? length : 3,
     shopAmount = length < 2 ? length : 2,
     restAmount = length < 1 ? length : 1;

    var eventList =
      (from n in Enumerable.Range(1, length)
       orderby random.Next()
       select n).Take(eventAmount);

    foreach (var n in eventList)
    {
      var node = (from line in data
                  where line[n].type == StageType.Battle
                  orderby random.Next()
                  select line[n]).First();
      node.type = StageType.Event;
    }

    //  상점 스테이지 삽입
    var shopList =
      (from n in Enumerable.Range(1, length)
       orderby random.Next()
       select n).Take(shopAmount);

    foreach (var n in shopList)
    {
      var node = (from line in data
                  where line[n].type == StageType.Battle
                  orderby random.Next()
                  select line[n]).First();
      node.type = StageType.Shop;
    }

    // 쉼터 스테이지 삽입
    var restList =
      (from n in Enumerable.Range(1, length)
       orderby random.Next()
       select n).Take(restAmount);

    foreach (var n in restList)
    {
      var node = (from line in data
                  where line[n].type == StageType.Battle
                  orderby random.Next()
                  select line[n]).First();
      node.type = StageType.Rest;
    }
    #endregion
  }

  // TODO 앞에 이동할 수 있는 노드가 있다면 이동불가능하도록
  private Vector2Int Focus(bool up)
  {
    var (px, py) = current;
    // 기초값 설정
    var result = beforeFocus;

    if (px == -1)
    {
      // 시작 노드일시 자유롭게 노드를 선택할 수 있게
      result = new(px + 1, up ?
        result.y != 0 ? result.y - 1 : Width - 1 :
        result.y != Width - 1 ? result.y + 1 : 0
      );
    }
    else
    {
      // 이후 노드에서는 위아래 3개 노드만 선택할 수 있게
      // 그러나 None 타입 노드일시 선택 불가(비어있는 노드)
      var range = 1;
      int[] choice =
        [.. (from node in data.Select((line, index) => (line, index))
         where node.index > py - range - 1 &&
           node.index < py + range + 1 &&
           node.line[px + 1].type != StageType.None
         select node.index)];

      if (choice.Length == 1)
      {
        result = new(px + 1, choice[0]);
      }
      else
      {
        var index = Array.FindIndex(choice, y => y == result.y);
        if (index == -1)
        {
          result = new(px + 1, choice[0]);
        }
        else
        {
          if (up) index = index == 0 ? choice.Length - 1 : index - 1;
          else index = index == choice.Length - 1 ? 0 : index + 1;

          result = new(px + 1, choice[index]);
        }
      }
    }

    return result;
  }

  private Vector2Int beforeFocus = new(0, 0);
  private List<int> path = [];
  private readonly List<Action<Player, Monster[]>> onNextBattle = [];
  private readonly int padSize = 8;

  public bool Render()
  {
  Render:
    Console.Clear();
    var (sx, sy) = Console.GetCursorPosition();
    var focus = beforeFocus;

    for (int y = 0; y < Width; y++)
    {
      var line = data[y];

      for (int x = 0; x < length; x++)
      {
        Console.SetCursorPosition(sx + x * padSize, sy + y * 2);
        var label = line[x].type.GetValue();

        if (focus.x == x && focus.y == y)
        {
          // ㅈ버그 진짜 ㅋㅋ
          AnsiConsole.Markup($"[black on white]{label}[/]");
        }
        else if (x < focus.x)
        {
          if (path[x] == y)
            AnsiConsole.Markup($"[cyan]{label}[/]");
          else
            AnsiConsole.Markup($"[gray]{label}[/]");
        }
        else
          AnsiConsole.Markup(label);
      }
      Console.WriteLine();
    }

    switch (Console.ReadKey().Key)
    {
      case ConsoleKey.UpArrow:
        beforeFocus = Focus(true);
        Console.SetCursorPosition(sx, sy);
        goto Render;

      case ConsoleKey.DownArrow:
        beforeFocus = Focus(false);
        Console.SetCursorPosition(sx, sy);
        goto Render;

      case ConsoleKey.Enter:
        // 스테이지 입장
        path.Add(focus.y);
        switch (data[focus.y][focus.x].type)
        {
          case StageType.Battle:
            var enemies = new List<Monster>();
            for (int i = 0; i < random.NextInt64(1, 4); i++)
            {
              var monster = new Monster(monsterPool[random.NextInt64(0, monsterPool.Length)]);

              enemies.Add(monster);
            }

            new Battle(Player, enemies).StartBattle();
            break;

          case StageType.Event:
            var targetEvent = eventPool[(int)random.NextInt64(1, eventPool.Length)];
            targetEvent?.Action(Player);
            break;

          case StageType.Shop:
            var itemCount = (int)random.NextInt64(1, itemPool.Length);
            Shop.EnterShop(new Shop(
              (from item in itemPool
               orderby random.Next()
               select item).Take(itemCount)
            ), Player);
            break;

          case StageType.Rest:
            GameManager.Game!.Rest(500);
            break;

          case StageType.Boss:
            var boss = new Monster(monsterPool[0]);
            boss.hp *= 3;
            new Battle(Player, boss).StartBattle();
            return true;

          default:
            break;
        }
        current = focus;
        beforeFocus = Focus(true);
        Console.SetCursorPosition(sx, sy);
        goto Render;

      default:
        Console.SetCursorPosition(sx, sy);
        goto Render;
    }
  }
}