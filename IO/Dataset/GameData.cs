using MessagePack;

namespace Starfall.IO.Dataset
{
  [MessagePackObject]
  public struct GameData
  {
    [Key(nameof(Name))]
    public string Name { get; set; } = "Chad";
    [Key(nameof(Level))]
    public int Level { get; set; } = 1;
    [Key(nameof(Job))]
    public string Job { get; set; } = "전사";
    [Key(nameof(Atk))]
    public float Atk { get; set; } = 10;
    [Key(nameof(Def))]
    public float Def { get; set; } = 5;
    [Key(nameof(Hp))]
    public float Hp { get; set; } = 100;
    [Key(nameof(Gold))]
    public int Gold { get; set; } = 1500;
    [Key(nameof(Inventory))]
    public GameDataItem[] Inventory { get; set; } = [
      new (){Name = "무쇠갑옷", Equip = true},
      new (){Name = "스파르타의창", Equip = true},
      new (){Name = "낡은검", Equip = false},
      new (){Name = "저주", Equip = false},
    ];

    public GameData()
    { }
  }

  [MessagePackObject]
  public struct GameDataItem
  {
    [Key(nameof(Name))]
    public string Name { get; set; }
    [Key(nameof(Equip))]
    public bool Equip { get; set; }
  }
}