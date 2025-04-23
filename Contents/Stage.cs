namespace Starfall.Contents;

public static class StageTypeUtil
{
  public static string GetValue(this StageType type) => type switch
  {
    StageType.None => "Err",
    StageType.Start => "Start",
    StageType.Battle => "Battle",
    StageType.Event => "Event",
    StageType.Shop => "Shop",
    StageType.Boss => "Boss",
    _ => "Err"
  };
}

public enum StageType
{
  None = 0,
  Start = 1,
  Battle = 2,
  Event = 3,
  Shop = 4,
  Boss = 5
}


public class StageNode()
{
  public StageType type;
  // 위쪽, 그대로, 아래쪽 노드로 이동 가능한지
}