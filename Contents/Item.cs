namespace Starfall.Contents
{
  public enum ItemType
  {
    Consumable,
    Material,
    Equipment,
    Special
  }
  public class Item
  {
    public string name;
    public string decription;
    public ItemType type;

    public Item(string name)
    {
      this.name = name;
      decription = "no description";
    }
  }
}