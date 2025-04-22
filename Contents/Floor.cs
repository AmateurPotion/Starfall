using Starfall.Utils;

namespace Starfall.Contents;

public class Floor
{
  private static readonly Random random = new();
  public readonly int length;
  public List<StageNode>[] data;
  public Vector2Int current = new();
  public int Height => data.Length;
  public Floor(int length = 5, int height = 3)
  {
    this.length = length + 3;
    data = new List<StageNode>[height];

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

    // 각 라인마다 1개 노드 삭제하기
    for (int x = 0; x < length; x++)
    {
      data[random.NextInt64(0, height)][x + 1].type = StageType.None;
    }

    //  이벤트 & 상점 스테이지 삽입
    int eventAmount = length < 3 ? length : 3,
     shopAmount = length < 2 ? length : 2;

    var eventList =
      (from n in Enumerable.Range(1, length)
       orderby random.Next()
       select n).Take(eventAmount);

    foreach (var n in eventList)
    {
      (from line in data
       where line[n].type == StageType.Battle
       orderby random.Next()
       select line[n]).GetEnumerator().Current.type = StageType.Event;
    }

    var shopList =
      (from n in Enumerable.Range(1, length)
       orderby random.Next()
       select n).Take(shopAmount);

    foreach (var n in shopList)
    {
      (from line in data
       where line[n].type == StageType.Battle
       orderby random.Next()
       select line[n]).GetEnumerator().Current.type = StageType.Shop;
    }
    #endregion
  }

  private Vector2Int Focus(bool up)
  {
    var (px, py) = current;
    if (px == 0)
    {
    }
    else
    {

    }

    return beforeFocus;
  }

  private Vector2Int beforeFocus = new(0, 0);

  public void Render()
  {
    Console.Clear();
    var (sx, sy) = Console.GetCursorPosition();
    var focus = new Vector2Int();

    switch (Console.ReadKey().Key)
    {
      case ConsoleKey.UpArrow:
        focus = Focus(true);
        break;

      case ConsoleKey.DownArrow:
        focus = Focus(false);
        break;

      case ConsoleKey.Enter:
        break;
    }


    for (int x = 0; x < length; x++)
    {

    }

    Render();
  }
}