using Starfall.IO;
using Starfall.IO.CUI;

namespace Starfall
{
  public class Program
  {
    public static void Main(params string[] args)
    {
      StorageController.Init();

    Menu:
      Console.Clear();
      InputManager.PrintTextFile("Starfall.Resources.intro.txt", ConsoleColor.DarkCyan, ConsoleColor.Green);
      switch (MenuUtil.OpenMenu(true, "새로운 여정", "데이터 불러오기", "다른 여정 참여"))
      {
        case 0:
          // 새로운 여정 - 새 게임
          break;

        case 1:
          // 데이터 불러오기 - 데이터 불러오기
          var saveList = StorageController.GetSaveNames();
          switch (MenuUtil.OpenMenu(true, [.. saveList, "뒤로가기"]))
          {
            case -1:
            case var i when i == saveList.Length:
              goto Menu;
          }
          break;

        case 2:
          // 다른 여정 참여 - 다른 여정 참여
          Console.WriteLine("개발중입니다.");
          Console.ReadKey();
          goto Menu;

        case -1: goto Menu;
      }


    }
  }
}