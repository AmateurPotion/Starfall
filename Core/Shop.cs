namespace Starfall.Core
{
  public class Shop
  {
    public List<ClassicItem> sellItems = [];
    public float sellRatio = 0.85f;

    public Shop(params string[] itemNames)
    {
      var itemDic = GameManager.items;

      foreach (var key in itemNames)
      {
        if (itemDic.TryGetValue(key, out ClassicItem value))
        {
          sellItems.Add(value);
        }
      }
    }
  }
}