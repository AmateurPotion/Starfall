using Starfall.Contents.Json;

namespace Starfall.Contents;

public class Dungeon
{
  public string label = "";
  public Floor[] floors;

  public Dungeon(string label, FloorData[] floorDatas)
  {
    this.label = label;
    floors = new Floor[floorDatas.Length];

    for (int i = 0; i < floorDatas.Length; i++)
    {
      floors[i] = new(5, 5)
      {

      };
    }
  }
}