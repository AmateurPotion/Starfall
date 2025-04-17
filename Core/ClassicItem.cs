namespace Starfall.Core
{
  public static class ClassicItemTypeUtill
  {
    public static string GetName(this ClassicItemType type) => type switch
    {
      ClassicItemType.None => "None",
      ClassicItemType.Weapon => "무기",
      ClassicItemType.Armor => "방어구",
      ClassicItemType.Accessory => "장신구",
      ClassicItemType.Etc => "기타",
      _ => "None"
    };
  }
  public enum ClassicItemType
  {
    None = 0,
    Weapon = 1,
    Armor = 2,
    Accessory = 3,
    Etc = 4
  }
  public struct ClassicItem
  {
    public static readonly Dictionary<string, ClassicItem> items = [];
    public ClassicItemType Type { get; set; } = ClassicItemType.None;
    public string Name { get; set; } = "undefined";
    public string Description { get; set; } = "no description";
    public int Atk { get; set; } = 0;
    public int Def { get; set; } = 0;
    public int Hp { get; set; } = 0;
    public int Price { get; set; } = 0;

    public ClassicItem()
    { }

    public static bool operator !=(ClassicItem item1, ClassicItem item2)
      => !item1.Equals(item2);
    public static bool operator ==(ClassicItem item1, ClassicItem item2)
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
}