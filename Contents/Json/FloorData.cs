namespace Starfall.Contents.Json;

public struct FloorData()
{
  // 층의 길이
  public int Length { get; set; } = 5;
  // 층의 넓이
  // 최솟값 5
  public int Width { get; set; } = 5;
  // 몬스터 목록 0번 인덱스는 보스 몬스터
  public string[] MobPool { get; set; } = [];
  // 상점, 이벤트, 전투 보상 등으로 받을 수 있는 아이템 목록
  public string[] ItemPool { get; set; } = [];
  // 이벤트방에서 랜덤으로 발생하는 이벤트 목록
  public string[] EventPool { get; set; } = [];
}