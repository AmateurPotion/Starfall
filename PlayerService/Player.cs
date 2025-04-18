using Starfall.Core;
using Starfall.IO.Dataset;

namespace Starfall.PlayerService
{
  public class Player
  {
    public string name = "Chad";
    public int level = 1;
    public string job = "전사";
    public float atk = 10;
    public float TrueAtk => atk + GetAddtionalAtk() + (level - 1) * 0.5f;
    public float def = 5;
    public float TrueDef => def + GetAddtionalDef() + (level - 1);
    public float hp = 100;
    public float TrueHp => hp + GetAddtionalHp();
    public int gold = 1500;

    public Dictionary<ClassicItem, bool> inventory = [];

    public float GetAddtionalAtk()
    {
      var addtional = 0f;
      foreach (var (item, equip) in inventory)
      {
        if (equip) addtional += item.Atk;
      }
      return addtional;
    }

    public float GetAddtionalDef()
    {
      var addtional = 0f;
      foreach (var (item, equip) in inventory)
      {
        if (equip) addtional += item.Def;
      }
      return addtional;
    }

    public float GetAddtionalHp()
    {
      var addtional = 0f;
      foreach (var (item, equip) in inventory)
      {
        if (equip) addtional += item.Hp;
      }
      return addtional;
    }

    public static implicit operator Player(GameData data)
      => new()
      {
        name = data.Name,
        level = data.Level,
        job = data.Job,
        atk = data.Atk,
        def = data.Def,
        hp = data.Hp,
        gold = data.Gold,
        inventory = data.Inventory.ToDictionary(v => GameManager.items[v.Name], v => v.Equip)
      };

    public static implicit operator GameData(Player player)
      => new()
      {
        Name = player.name,
        Level = player.level,
        Job = player.job,
        Atk = player.atk,
        Def = player.def,
        Hp = player.hp,
        Gold = player.gold,
        Inventory = [.. from pair in player.inventory select new GameDataItem()
        {
          Name = GameManager.items.FirstOrDefault(item => item.Value == pair.Key).Key,
          Equip = pair.Value
        }]
      };
  }
}