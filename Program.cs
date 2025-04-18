using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.IO.Dataset;

namespace Starfall
{
  public class Program
  {
    public static void Main(params string[] args)
    {
      GameManager.Init();
    Menu:
      Console.Clear();
      ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt", ConsoleColor.DarkMagenta, ConsoleColor.Green);
      Console.WriteLine();

      StorageController.SetSaveName("default");
      switch (MenuUtil.OpenMenu("새로운 여정", "데이터 불러오기", "다른 여정 참여"))
      {
        case 0:
          // 새로운 여정 - 새 게임
          GameManager.StartGame(new());
          break;

        case 1:
          // 데이터 불러오기 - 데이터 불러오기

          GameManager.StartGame(StorageController.LoadBinary<GameData>("data"));

          break;

        case 2:
          // 다른 여정 참여 - 다른 여정 참여
          GameManager.JoinGame();
          Console.WriteLine("개발중입니다.");
          Console.ReadKey();
          goto Menu;

        case -1: goto Menu;
      }


    }
  }
}