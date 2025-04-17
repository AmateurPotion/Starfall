namespace Starfall.IO.Dataset
{
  public struct GameData
  {
    public string pageName { get; set; } = "";
    public string name { get; set; } = "Chad";
    public int level { get; set; } = 1;
    public string job { get; set; } = "전사";
    public int atk { get; set; } = 10;
    public int def { get; set; } = 5;
    public int hp { get; set; } = 100;
    public int gold { get; set; } = 1500;
    public GameDataItem[] inventory { get; set; } = [
      new (){name = "무쇠갑옷", equip = true},
      new (){name = "스파르타의창", equip = true},
      new (){name = "낡은검", equip = false},
      new (){name = "저주", equip = false},
    ];

    public GameData()
    { }
  }

  public struct GameDataItem
  {
    public string name { get; set; }
    public bool equip { get; set; }
  }
}