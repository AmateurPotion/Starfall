namespace Starfall.IO.Dataset
{
  public struct GameData
  {
    public string Name { get; set; } = "Chad";
    public int Level { get; set; } = 1;
    public string Job { get; set; } = "전사";
    public float Atk { get; set; } = 10;
    public float Def { get; set; } = 5;
    public float Hp { get; set; } = 100;
    public int Gold { get; set; } = 1500;
    public GameDataItem[] Inventory { get; set; } = [
      new (){Name = "무쇠갑옷", Equip = true},
      new (){Name = "스파르타의창", Equip = true},
      new (){Name = "낡은검", Equip = false},
      new (){Name = "저주", Equip = false},
    ];

    public GameData()
    { }

    public readonly void ToBinary()
    {

    }
  }

  public struct GameDataItem
  {
    public string Name { get; set; }
    public bool Equip { get; set; }
  }
}