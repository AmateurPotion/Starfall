using MessagePack;
using Starfall.PlayerService;

namespace Starfall.Contents.Binary;

[MessagePackObject]
public struct GameData
{
  [Key(nameof(Name))]
  public string Name { get; set; } = "Chad";
  [Key(nameof(Level))]
  public int Level { get; set; } = 1;
  [Key(nameof(Job))]
  public Job Job { get; set; } = Job.None;
  [Key(nameof(Atk))]
  public float Atk { get; set; } = 10;
  [Key(nameof(Def))]
  public float Def { get; set; } = 5;
  [Key(nameof(Hp))]
  public float Hp { get; set; } = 100;
  [Key(nameof(Mp))]
  public float Mp { get; set; } = 50;
  [Key(nameof(Gold))]
  public int Gold { get; set; } = 1500;
  [Key(nameof(Skills))]
  public string[] Skills { get; set; } = [];
  [Key(nameof(Inventory))]
  public GameItemData[] Inventory { get; set; } = [];

  public GameData()
  { }
}

[MessagePackObject]
public struct GameItemData
{
  [Key(nameof(Name))]
  public string Name { get; set; }
  [Key(nameof(Equip))]
  public int Equip { get; set; }
}