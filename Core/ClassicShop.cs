using System.Collections;

namespace Starfall.Core
{
  public class ClassicShop : IEnumerable<ClassicItem>
  {
    public List<ClassicItem> sellItems = [];
    public float sellRatio = 0.85f;

    public ClassicShop(params string[] itemNames)
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

    public ClassicItem this[int i]
    {
      get => sellItems[i];
      set => sellItems[i] = value;
    }

    public IEnumerator<ClassicItem> GetEnumerator()
      => sellItems.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
      => GetEnumerator();
  }
}