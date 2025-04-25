namespace Starfall.Contents.Json;

public struct MonsterData()
{
  public string Name { get; set; } = "MonsterName";
  public float Hp { get; set; } = 10;
  public float Atk { get; set; } = 3;
  public float Def { get; set; } = 0;
  public int Level { get; set; } = 1;
  public int RewardGold { get; set; } = 100;
  public float CriticalChance { get; set; } = 15f;
  public float CriticalDamageMultiplyer { get; set; } = 160f;
  public float EvasionChance { get; set; } = 10f;
}