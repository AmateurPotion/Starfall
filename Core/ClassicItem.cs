namespace Starfall.Core
{
  public enum ClassicItemType
  {

  }
  public struct ClassicItem
  {
    public static readonly Dictionary<string, ClassicItem> items = [];
    public string name { get; set; } = "undefined";
    public string description { get; set; } = "no description";
    public int atk { get; set; } = 0;
    public int def { get; set; } = 0;
    public int hp { get; set; } = 0;
    public int price { get; set; } = 0;

    public ClassicItem()
    {

    }
  }
}