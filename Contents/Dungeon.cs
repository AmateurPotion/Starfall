using Spectre.Console;
using Starfall.Contents.Json;

namespace Starfall.Contents;

public class Dungeon
{
  private static Dictionary<string, Item> Items => GameManager.items;
  private static Dictionary<string, Event> Events => GameManager.events;
  private static Dictionary<string, MonsterData> Monsters => GameManager.monsters;
  public float requireDef = 0;
  public float requireAtk = 0;
  public string label = "";
  public Floor[] floors;

  public Dungeon(string label, FloorData[] floorDatas)
  {
    this.label = label;
    floors = new Floor[floorDatas.Length];

    for (int i = 0; i < floorDatas.Length; i++)
    {
      var data = floorDatas[i];

      floors[i] = new(
        [.. (from key in data.ItemPool where Items.ContainsKey(key) select Items[key])],
        [.. (from key in data.MobPool where Monsters.ContainsKey(key) select Monsters[key])],
        [.. (from key in data.EventPool where Events.ContainsKey(key) select Events[key])],
         data.Length, Math.Max(data.Width, 5));
    }
  }

  public void Join()
  {
    foreach (var floor in floors)
    {
      floor.Render();
    }
  }
}