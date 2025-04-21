using System.Collections;

namespace Starfall.Contents.Json;
public class Shop : IEnumerable<Item>
{
  public List<Item> sellItems = [];
  public float sellRatio = 0.85f;

  public Shop(params string[] itemNames)
  {
    var itemDic = GameManager.items;

    foreach (var key in itemNames)
    {
      if (itemDic.TryGetValue(key, out var value))
      {
        sellItems.Add(value);
      }
    }
  }

  public Item this[int i]
  {
    get => sellItems[i];
    set => sellItems[i] = value;
  }

  public IEnumerator<Item> GetEnumerator()
    => sellItems.GetEnumerator();

  IEnumerator IEnumerable.GetEnumerator()
    => GetEnumerator();
}