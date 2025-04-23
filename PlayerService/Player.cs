using Starfall.Contents.Binary;
using Starfall.Contents.Json;
using static Starfall.Core.CreatePlayer;

namespace Starfall.PlayerService
{
  public partial class Player
  {
    public string name = "Chad";

    public JobName job = JobName.None;
    public float atk = 10;
    public float TrueAtk => atk + GetAddtionalAtk() + (level - 1) * 0.5f;
    public float def = 5;
    public float TrueDef => def + GetAddtionalDef() + (level - 1);
    public float hp = 100;
    public float mp = 50;
    public float TrueHp => hp + GetAddtionalHp();
    public float TrueMp => mp + GetAddtionalMp();
    public int gold = 1500;

    public Dictionary<Item, int> inventory = [];

    public float GetAddtionalAtk()
    {
      var addtional = 0f;
      foreach (var (item, equip) in inventory)
      {
        if (item.Type != ItemType.Consumable && equip == 1) addtional += item.Atk;
      }
      return addtional;
    }

    public float GetAddtionalDef()
    {
      var addtional = 0f;
      foreach (var (item, equip) in inventory)
      {
        if (item.Type != ItemType.Consumable && equip == 1) addtional += item.Def;
      }
      return addtional;
    }

    public float GetAddtionalHp()
    {
      var addtional = 0f;
      foreach (var (item, equip) in inventory)
      {
        if (item.Type != ItemType.Consumable && equip == 1) addtional += item.Hp;
      }
      return addtional;
    }

    public float GetAddtionalMp()
    {
      var addtional = 0f;
      foreach (var (item, equip) in inventory)
      {
        if (item.Type != ItemType.Consumable && equip == 1) addtional += item.Mp;
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
    mp = data.Mp,
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
        Mp = player.mp,
        Gold = player.gold,
        Inventory = [.. from pair in player.inventory select new GameDataItem()
        {
          Name = GameManager.items.FirstOrDefault(item => item.Value == pair.Key).Key,
          Equip = pair.Value
        }]
      };
  }
}