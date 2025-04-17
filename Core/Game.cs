using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.IO.Dataset;
using Starfall.PlayerService;

namespace Starfall.Core
{
  public class Game
  {
    public Player player;
    private Action prevAct = () => { };
    public Game(GameData data)
    {
      player = data;
    }

    public void Start()
    {
      prevAct = OpenHub;
      OpenHub();
    }

    public void OpenHub()
    {
    Render:
      Console.Clear();
      InputManager.PrintTextFile("Starfall.Resources.pages.Hub.txt");

      switch (MenuUtil.OpenMenu(true, "1. 상태 보기", "2. 인벤토리", "3. 상점"))
      {
        case 0:
          // 상태 보기
          OpenStatus();
          break;

        case 1:
          // 인벤토리
          OpenInventory();
          break;

        case 2:
          // 상점
          break;

        // 취소시 -1
        default: goto Render;
      }
    }

    private void OpenStatus()
    {
      Console.Clear();
      Console.WriteLine($"""
      상태보기
      캐릭터의 정보가 표시됩니다.
      Lv. {player.level:D2}
      {player.name} ( {player.job} )
      공격력 : {player.atk}
      방어력 : {player.def}
      체 력 : {player.health}
      Gold : {player.gold} G
      """);

      Console.WriteLine();
      MenuUtil.OpenMenu("나가시려면 엔터를 클릭하세요.");
      prevAct();
    }

    private void OpenInventory()
    {
      Console.Clear();
      Console.WriteLine("""
      인벤토리
      보유 중인 아이템을 관리할 수 있습니다.

      [아이템 목록]
      """);
      foreach (var (item, equip) in player.inventory)
      {
        Console.Write($"- {(equip ? "[E]" : "")}{item.name} | ");
        var stats = new List<string>();
        if (item.atk != 0) stats.Add("공격력 " + (item.atk > 0 ? "+" + item.atk : item.atk));
        if (item.def != 0) stats.Add("방어력 " + (item.def > 0 ? "+" + item.def : item.def));
        if (item.hp != 0) stats.Add("생명력 " + (item.hp > 0 ? "+" + item.hp : item.hp));
        Console.Write(string.Join(" / ", stats));
        Console.WriteLine(" | " + item.description);
      }

      switch (MenuUtil.OpenMenu("장착 관리", "나가기"))
      {
        case 1:
          // Edit equip
          break;

        // 나가기
        case 0:
        default: break;
      }

      prevAct();
    }
  }
}