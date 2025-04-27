using Spectre.Console;
using Starfall.Contents;
using Starfall.Contents.Binary;
using Starfall.Contents.Json;
using Starfall.IO;
using Starfall.IO.CUI;

namespace Starfall.PlayerService
{
  public partial class Player
  {
    #region States
    public string name = "Chad";
    public Job job = Job.None;
    public float atk = 10;
    public float TrueAtk => atk + GetAddtionalAtk() + (level - 1) * 0.5f;
    public float def = 5;
    public float TrueDef => def + GetAddtionalDef() + (level - 1);
    public float hp = 100;
    public float mp = 50;
    public float TrueHp => hp + GetAddtionalHp();
    public float TrueMp => mp + GetAddtionalMp();
    public int gold = 1500;
    #endregion
    public Dictionary<Item, int> inventory = [];
    public List<Skill> skills = [];

    // ?? wtf 
    public float GetStatAsString(string stat) => stat switch
    {
      "Atk" => TrueAtk,
      "Def" => TrueDef,
      "Hp" => TrueHp,
      "Mp" => TrueMp,
      _ => 0
    };

    #region Addtional
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

    #endregion

    #region Implicit

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
      inventory = data.Inventory.ToDictionary(v => GameManager.items[v.Name], v => v.Equip),
      skills = [..(
        from key in data.Skills
        where GameManager.skills.ContainsKey(key)
        select GameManager.skills[key]
      )]
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
        Inventory = [.. (from pair in player.inventory select new GameItemData()
        {
          Name = GameManager.items.FirstOrDefault(item => item.Value == pair.Key).Key,
          Equip = pair.Value
        })],
        Skills = [.. (from skill in player.skills select skill.name)]
      };
    #endregion

    #region StaticUtil
    public static void CreateNew()
    {
    Start:
      // ===========================
      Console.Clear();
      ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt");
      Console.WriteLine();
      // ===========================

      // 이름 설정
      AnsiConsole.MarkupLine("""
            원하시는 이름을 입력해주세요.
            """);

      Console.WriteLine();
      string input = Console.ReadLine() ?? "";

      if (string.IsNullOrEmpty(input))
      {
        AnsiConsole.MarkupLine("비어있지 않은 문자열을 입력해주세요.");
        MenuUtil.OpenMenu("확인");
        goto Start;
      }

      AnsiConsole.MarkupLine($"""
            
            입력하신 이름은 [[{input}]] 입니다.

            """);

      string name = input;

      // ===========================
      // 메뉴 리스트 설정
      switch (MenuUtil.OpenMenu("이 이름이 좋아요!", "마음이 바뀌었어요...", "메인 메뉴로"))
      {
        case 0:
          // 저장
          StorageController.SetSaveName($"{name}");
          var data = new GameData
          {
            Name = name
          };
          SetPlayerJob(data);
          break;

        case 1:
          CreateNew();
          break;

        case 2:
        case -1: GameManager.EnterMain(); break;
      }
      // ===========================
    }

    private static void SetPlayerJob(GameData data)
    {
      // ===========================
      Console.Clear();
      ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt");
      Console.WriteLine();
      // ===========================

      AnsiConsole.MarkupLine($"""
            
            원하시는 직업을 선택해주세요.

            """);


      // 메뉴 리스트 Linq 구문 이용해서 압축
      var jobNames = (
          from key in Enum.GetValues<Job>()
          where key != Job.None
          select key.GetName() + " / " + key.GetDescription()
      ).ToArray();

      int selection = MenuUtil.OpenMenu(jobNames);

      if (selection < 0) CreateNew();
      else
      {
        data.Job = (Job)(selection + 1);
        // 메뉴 씬으로 이동
        GameManager.StartGame(data);
      }
      // ===========================
    }
    #endregion
  }

}
