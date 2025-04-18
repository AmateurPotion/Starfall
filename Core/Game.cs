using System.Text;
using Spectre.Console;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.IO.Dataset;
using Starfall.PlayerService;

namespace Starfall.Core
{
  public class Game(GameData data)
  {
    public Player player = data;
    public Shop shop = new("수련자갑옷", "무쇠갑옷", "스파르타의갑옷", "낡은검", "청동도끼", "스파르타의창");
    private Func<bool> act = () => false;

    public void Start()
    {
      act = OpenHub;
      // 메소드 스택오버플로우를 막기위해 for문으로 구현했다.
      for (bool exit = OpenHub(); !exit; exit = act())
      { }
    }

    public bool OpenHub()
    {
      Console.Clear();
      ConsoleUtil.PrintTextFile("Starfall.Resources.pages.Hub.txt");

      var select = MenuUtil.OpenMenu(
        "[#d1949e]1[/]. 상태 보기", "[#d1949e]2[/]. 인벤토리",
        "[#d1949e]3[/]. 상점", "[#d1949e]4[/]. 던전입장",
        "[#d1949e]5[/]. 휴식하기", "[#d1949e]6[/]. 저장하기",
        "[#d1949e]7[/]. 게임 종료");

      act = select switch
      {
        // 상태보기
        0 => OpenStatus,
        // 인벤토리
        1 => OpenInventory,
        // 상점
        2 => () => OpenShop(shop),
        // 던전입장
        3 => JoinDungeon,
        // 휴식하기
        4 => Rest,
        // 저장하기
        5 => Save,
        _ => act
      };

      // 게임 종료 선택했을 때 게임 종료
      return select == 6;
    }

    private bool OpenStatus()
    {
      string StatView(float val) => val != 0 ?
        "([#f5b83d]" + (val > 0 ? "+" : "-") + $"[/][#d1949e]{Math.Abs(val)}[/]" + ")"
      : "";

      Console.Clear();
      AnsiConsole.MarkupLine($"""
      상태보기
      캐릭터의 정보가 표시됩니다.
      Lv. [#d1949e]{player.level:D2}[/]
      {player.name} ( {player.job} )
      공격력 : [#d1949e]{player.TrueAtk}[/] {StatView(player.GetAddtionalAtk())}
      방어력 : [#d1949e]{player.TrueDef}[/] {StatView(player.GetAddtionalDef())}
      체 력 : [#d1949e]{player.TrueHp}[/] {StatView(player.GetAddtionalHp())}
      Gold : [#d1949e]{player.gold}[/] G
      """);

      Console.WriteLine();
      MenuUtil.OpenMenu("[#d1949e]0[/]. 나가기");
      act = OpenHub;
      return false;
    }

    private bool OpenInventory()
    {
      Console.Clear();
      AnsiConsole.MarkupLine("""
      인벤토리
      보유 중인 아이템을 관리할 수 있습니다.

      [[아이템 목록]]
      """);

      foreach (var (item, equip) in player.inventory)
      {
        var option = new StringBuilder();

        option.Append($"- {item.Type.GetName()} / {(equip ? "[[E]]" : "")}{item.Name} | ");
        var stats = new List<string>();
        if (item.Atk != 0) stats.Add("공격력 " + (item.Atk > 0 ? "+" + item.Atk : item.Atk));
        if (item.Def != 0) stats.Add("방어력 " + (item.Def > 0 ? "+" + item.Def : item.Def));
        if (item.Hp != 0) stats.Add("생명력 " + (item.Hp > 0 ? "+" + item.Hp : item.Hp));
        option.Append(string.Join(" / ", stats));
        option.Append(" | " + item.Description);

        AnsiConsole.MarkupLine(option.ToString());
      }

      switch (MenuUtil.OpenMenu("\n장착 관리", "나가기"))
      {
        case 0:
          // Edit equip
          act = EditInventory;
          return false;

        // 나가기
        case 1:
        default:
          act = OpenHub;
          return false;
      }
    }

    // 아이템 장착 메뉴 이전 선택값 저장
    private int savedIndex = 0;
    private bool EditInventory()
    {
      Console.Clear();
      AnsiConsole.MarkupLine("""
      인벤토리 - 장착 관리
      보유 중인 아이템을 관리할 수 있습니다.

      [[아이템 목록]]
      """);
      var menu = new List<string>();
      var items = new List<ClassicItem>();
      int index = 0;
      foreach (var (item, equip) in player.inventory)
      {
        var option = new StringBuilder();
        option.Append($"- {index + 1} / {item.Type.GetName()} / {(equip ? "[[E]]" : "")}{item.Name} | ");
        var stats = new List<string>();
        if (item.Atk != 0) stats.Add("공격력 " + (item.Atk > 0 ? "+" : "") + item.Atk);
        if (item.Def != 0) stats.Add("방어력 " + (item.Def > 0 ? "+" : "") + item.Def);
        if (item.Hp != 0) stats.Add("생명력 " + (item.Hp > 0 ? "+" : "") + item.Hp);
        option.Append(string.Join(" / ", stats));
        option.Append(" | " + item.Description);

        items.Add(item);
        index++;
        menu.Add(option.ToString());
      }

      this.savedIndex = MenuUtil.OpenMenu(this.savedIndex, false, [.. menu, "\n0. 나가기"]);

      switch (this.savedIndex)
      {
        case var i when i > -1 && i < index:
          var item = items[i];
          if (player.inventory[item] = !player.inventory[item])
          {
            var type = item.Type;
            foreach (var (target, equip) in player.inventory)
            {
              if (equip && target.Type == type && target != item)
              {
                player.inventory[target] = false;
              }
            }
          }
          act = EditInventory;
          return false;

        default:
          act = OpenInventory;
          this.savedIndex = 0;
          return false;
      }
    }

    private static string StatStr(float value) => (value > 0 ? "+" : "-") + Math.Abs(value);
    public bool OpenShop(Shop shop)
    {
      Console.Clear();
      AnsiConsole.MarkupLine($"""
      상점
      아이템을 구매 / 판매할 수 있는 상점입니다.
      [[보유 골드]]
      {player.gold} G
      [[아이템 목록]]
      """);

      var menu = new List<string>();
      var index = 0;
      foreach (var item in shop)
      {
        var option = new StringBuilder($"- {++index} ");

        option.Append($"{item.Type} / {item.Name} |");
        var stats = new List<string>();
        if (item.Atk != 0) stats.Add("공격력 " + StatStr(item.Atk));
        if (item.Def != 0) stats.Add("방어력 " + StatStr(item.Def));
        if (item.Hp != 0) stats.Add("생명력 " + StatStr(item.Hp));
        option.Append(string.Join(" / ", stats));
        option.Append($" | {item.Description} | ");
        option.Append(player.inventory.ContainsKey(item) ? "구매완료" : item.Price + " G");
      }

      this.savedIndex = MenuUtil.OpenMenu(this.savedIndex, false, [.. menu, "\n0. 나가기"]);

      switch (this.savedIndex)
      {
        case var i when i > 0 && i < index:
          // 아이템 선택
          act = () => SelectItemOnShop(shop, shop.sellItems[i]);
          break;

        default:
          // 뒤로가기, 취소시 허브로 가기
          act = OpenHub;
          return false;
      }


      return false;
    }

    public bool SelectItemOnShop(Shop shop, ClassicItem item)
    {
      Console.Clear();
      var equip = "";
      if (player.inventory.TryGetValue(item, out var e) && e)
      {
        equip = "[[E]]";
      }
      AnsiConsole.MarkupLine($"""
      {equip} {item.Type.GetName()} - {item.Name}
      {item.Description}
      공격력 {StatStr(item.Atk)}
      방어력 {StatStr(item.Def)}
      생명력 {StatStr(item.Hp)}

      """);
      if (player.inventory.ContainsKey(item))
      {
        // 아이템이 있을 시 판매
        var sellPrice = (int)Math.Round(shop.sellRatio * item.Price);
        if (MenuUtil.OpenMenu($"판매하기 / {sellPrice} G", "뒤로가기") == 0)
        {
          player.gold += sellPrice;
          player.inventory.Remove(item);
          AnsiConsole.MarkupLine($"\n {sellPrice} G에 {item.Name}을 판매했습니다.\n");
          MenuUtil.OpenMenu("확인");
        }
      }
      else
      {
        // 아이템이 없을 시 구매
        if (MenuUtil.OpenMenu($"구매하기 / {item.Price} G {(player.gold <= item.Price ? "(골드부족)" : "")}", "뒤로가기") == 0)
        {
          if (player.gold >= item.Price)
          {
            player.gold -= item.Price;
            player.inventory.Add(item, false);
            AnsiConsole.MarkupLine($"\n구매를 완료했습니다.");
          }
          else AnsiConsole.MarkupLine("\nGold 가 부족합니다.");

          MenuUtil.OpenMenu("\n확인");
        }
      }

      // 선택했을시 다시 상점으로
      act = () => OpenShop(shop);
      return false;
    }

    public bool JoinDungeon()
    {
      return false;
    }

    public bool Rest()
    {
      return false;
    }

    public bool Save()
    {
      return false;
    }
  }
}