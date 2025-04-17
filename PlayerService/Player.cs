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
    public float gold = 1500;

    public Dictionary<ClassicItem, bool> inventory = [];

    public float GetAddtionalAtk()
    {
      var addtional = 0;
      foreach (var (item, equip) in inventory)
      {
        if (equip) addtional += item.Atk;
      }
      return addtional;
    }

    public float GetAddtionalDef()
    {
      var addtional = 0;
      foreach (var (item, equip) in inventory)
      {
        if (equip) addtional += item.Def;
      }
      return addtional;
    }

    public float GetAddtionalHp()
    {
      var addtional = 0;
      foreach (var (item, equip) in inventory)
      {
        if (equip) addtional += item.Hp;
      }
      return addtional;
    }

    public static implicit operator Player(GameData data)
      => new()
      {
        name = data.name,
        level = data.level,
        job = data.job,
        atk = data.atk,
        def = data.def,
        hp = data.hp,
        gold = data.gold,
        inventory = data.inventory.ToDictionary(v => GameManager.items[v.name], v => v.equip)
      };
  }
}