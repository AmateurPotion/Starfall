using Starfall.Core;
using Starfall.IO.Dataset;

namespace Starfall.PlayerService
{
  public class Player
  {
    public string name = "Chad";
    public int level = 1;
    public string job = "전사";
    public int atk = 10;
    public int def = 5;
    public int health = 100;
    public int gold = 1500;

    public Dictionary<ClassicItem, bool> inventory = [];

    public static implicit operator Player(GameData data)
      => new()
      {
        name = data.name,
        level = data.level,
        job = data.job,
        atk = data.atk,
        def = data.def,
        health = data.health,
        gold = data.gold,
        inventory = data.inventory.ToDictionary(v => GameManager.items[v.name], v => v.equip)
      };
  }
}