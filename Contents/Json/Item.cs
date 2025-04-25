namespace Starfall.Contents.Json;
public static class ItemTypeUtill
{
  public static string GetName(this ItemType type) => type switch
  {
    ItemType.None => "None",
    ItemType.Weapon => "무기",
    ItemType.Armor => "방어구",
    ItemType.Accessory => "장신구",
    ItemType.Consumable => "소모품",
    ItemType.Etc => "기타",
    _ => "None"
  };
}

public enum ItemType
{
  None = 0,
  Weapon = 1,
  Armor = 2,
  Accessory = 3,
  Consumable = 4,
  Etc = 5
}

public enum ItemRarity
{
  Common = 1,
  Special = 2,
  Eternal = 3,
  Unique = 4
}

public struct Item
{
  public static readonly Dictionary<string, Item> items = [];
  public ItemType Type { get; set; } = ItemType.None;
  public string Name { get; set; } = "undefined";
  public string Description { get; set; } = "no description";
  public float Atk { get; set; } = 0;
  public float Def { get; set; } = 0;
  public float Hp { get; set; } = 0;
  public float Mp { get; set; } = 0;
  public int Price { get; set; } = 0;
  public ItemRarity Rarity { get; set; } = ItemRarity.Common;

  public Item()
  { }

  public static bool operator !=(Item item1, Item item2)
    => !item1.Equals(item2);
  public static bool operator ==(Item item1, Item item2)
    => item1.Equals(item2);

  public override readonly bool Equals(object? obj)
  {
    return base.Equals(obj);
  }

  public override readonly int GetHashCode()
  {
    return base.GetHashCode();
  }
}
