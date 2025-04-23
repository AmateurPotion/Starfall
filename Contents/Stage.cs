namespace Starfall.Contents;

public static class StageTypeUtil
{
  public static string GetValue(this StageType type) => type switch
  {
    StageType.None => "",
    StageType.Start => "시작",
    StageType.Battle => "전투",
    StageType.Event => "이벤트",
    StageType.Shop => "상점",
    StageType.Boss => "보스전",
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