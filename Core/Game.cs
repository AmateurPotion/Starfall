using System.Text;
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
        "1. 상태 보기", "2. 인벤토리", "3. 상점",
        "4. 던전입장", "5. 휴식하기", "6. 저장하기",
        "7. 게임 종료");

      act = select switch
      {
        // 상태보기
        0 => OpenStatus,
        // 인벤토리
        1 => OpenInventory,
        // 상점
        2 => OpenBuyShop,
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
      string StatView(float val) => val != 0 ? val > 0 ?
        $"(+{val})" : $"({val})" : "";

      Console.Clear();
      Console.WriteLine($"""
      상태보기
      캐릭터의 정보가 표시됩니다.
      Lv. {player.level:D2}
      {player.name} ( {player.job} )
      공격력 : {player.TrueAtk} {StatView(player.GetAddtionalAtk())}
      방어력 : {player.TrueDef} {StatView(player.GetAddtionalDef())}
      체 력 : {player.TrueHp} {StatView(player.GetAddtionalHp())}
      Gold : {player.gold} G
      """);

      Console.WriteLine();
      MenuUtil.OpenMenu("나가시려면 엔터를 클릭하세요.");
      act = OpenHub;
      return false;
    }

    private bool OpenInventory()
    {
      Console.Clear();
      Console.WriteLine("""
      인벤토리
      보유 중인 아이템을 관리할 수 있습니다.

      [아이템 목록]
      """);

      foreach (var (item, equip) in player.inventory)
      {
        Console.Write($"- {item.Type.GetName()} / {(equip ? "[E]" : "")}{item.Name} | ");
        var stats = new List<string>();
        if (item.Atk != 0) stats.Add("공격력 " + (item.Atk > 0 ? "+" + item.Atk : item.Atk));
        if (item.Def != 0) stats.Add("방어력 " + (item.Def > 0 ? "+" + item.Def : item.Def));
        if (item.Hp != 0) stats.Add("생명력 " + (item.Hp > 0 ? "+" + item.Hp : item.Hp));
        Console.Write(string.Join(" / ", stats));
        Console.WriteLine(" | " + item.Description);
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
    private int prevIndex = 0;
    private bool EditInventory()
    {
      Console.Clear();
      Console.WriteLine("""
      인벤토리 - 장착 관리
      보유 중인 아이템을 관리할 수 있습니다.

      [아이템 목록]
      """);
      var menu = new List<string>();
      var items = new List<ClassicItem>();
      int index = 0;
      foreach (var (item, equip) in player.inventory)
      {
        var option = new StringBuilder();
        option.Append($"- {index + 1} / {item.Type.GetName()} / {(equip ? "[E]" : "")}{item.Name} | ");
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

      prevIndex = MenuUtil.OpenMenu(prevIndex, false, [.. menu, "\n0. 나가기"]);

      switch (prevIndex)
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
          prevIndex = 0;
          return false;
      }
    }

    public bool OpenBuyShop()
    {

      return false;
    }

    public bool OpenSellhop()
    {
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