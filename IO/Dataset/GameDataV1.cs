namespace Starfall.IO.Dataset
{
  public struct GameDataV1 : IGameData
  {
    public int Version { get; set; } = 1;
    public string playerName { get; set; } = "undefined";
    public int level { get; set; }
    public string playerJob { get; set; } = "undefined";
    public float atk { get; set; }
    public float def { get; set; }
    public float health;
    public float maxHealth;
    public int gold;

    public GameDataV1()
    { }
  }
}