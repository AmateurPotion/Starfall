using Starfall.Utils;

namespace Starfall.Contents;

public class Floor
{
  public readonly int length;
  public List<StageNode>[] data;
  public Vector2Int current = new();
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
      for (int x = 0; x < length; x++)
      {
        line.Add(x switch
        {
          // 시작 노드
          0 => new() { type = StageType.Start },
          // 보스 노드
          var i when i == length - 2 => lastShopNode,
          var i when i == length - 1 => bossNode,
          // 무작위 노드 생성
          _ => new()
          {
            type = StageType.Battle,
            middle = true,
            top = y != 0,
            bottom = y != data.Length - 1
          }
        });
      }

      // 일부 노드 삭제하기
      int deleteAmount = 3;
      for (int x = 0; x < length && deleteAmount > 0; x++)
      {

      }

      //  이벤트 & 상점 스테이지 삽입
      int eventAmount = 3, shopAmount = 3;
    }

    #endregion
  }

  public void Render()
  {
    Console.Clear();

    for (int x = 0; x < length; x++)
    {
    }
  }
}