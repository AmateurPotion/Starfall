using Spectre.Console;
using Starfall.Contents.Binary;
using Starfall.IO;
using Starfall.IO.CUI;

namespace Starfall.Core
{
  public static class CreatePlayer
  {
    #region Job Name
    public enum JobName
    {
      None = 0,
      Warrior = 1,
      Wizard = 2,
      Rogue = 3,
    }

    /// <summary>
    /// ex) JobName.None.GetJobNameToKor()
    /// </summary>
    /// <param name="className"></param>
    /// <returns></returns>
    public static string GetJobNameToKor(this JobName className) => className switch
    {
      JobName.None => "없음",
      JobName.Warrior => "전사",
      JobName.Wizard => "마법사",
      JobName.Rogue => "도적",
      _ => "",
    };
    #endregion

    public static void CreateNewPlayer()
    {
      SetPlayerName();
    }

    private static void SetPlayerName()
    {
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

      AnsiConsole.MarkupLine("""
            
      입력하신 이름은 [{input}]입니다.

      """);

      string name = input;

      // ===========================
      // 메뉴 리스트 설정
      switch (MenuUtil.OpenMenu("저장", "다시 입력", "메인 메뉴로"))
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
          SetPlayerName();
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

      // 메뉴 리스트 Linq 구문 이용해서 압축
      var jobNames = (
          from key in Enum.GetValues<JobName>()
          where key != JobName.None
          select key.GetJobNameToKor()
      ).ToArray();

      int selection = MenuUtil.OpenMenu(jobNames);

      if (selection < 0) SetPlayerName();
      else
      {
        data.Job = (JobName)(selection + 1);
        // 메뉴 씬으로 이동
        GameManager.StartGame(data);
      }
      // ===========================
    }
  }
}
