using Starfall.Utils;

namespace Starfall.Contents;

public class Floor
{
  public readonly int length;
  public List<StageNode>[] data;
  public Vector2Int current = new();
  public Floor(int length = 5, int height = 3)
  {
    this.length = length;
    data = new List<StageNode>[height];

    #region GenerateFloorMap
    // TODO 가장 위 라인, 가장 아래 노드 라인일 경우 위나 아래로 못 이동하게 구현
    var bossNode = new StageNode() { type = StageType.Boss };

    for (int y = 0; y < data.Length; y++)
    {
      // 라인 생성
      var line = data[y] = [];

      for (int x = 0; x < length; x++)
      {
        line.Add(x switch
        {
          // 시작 노드
          0 => new() { type = StageType.Start },
          // 보스 노드
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