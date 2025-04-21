using Spectre.Console;
using Starfall.IO;
using Starfall.IO.CUI;

namespace Starfall.Core
{
    public class CreatePlayer
    {
        #region Job Name
        public enum JobName
        {
            None = -1,
            Warrior = 0,
            Wizard = 1,
            Rogue = 2,
        }

        public static string GetJobNameToKor(JobName className) => className switch
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

        static void SetPlayerName()
        {
            Console.Clear();
            ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt", ConsoleColor.DarkMagenta, ConsoleColor.Green);
            Console.WriteLine();

            // 이름 설정
            Console.WriteLine(
                "\n" + 
                "원하시는 이름을 입력해주세요.");

            Console.WriteLine();
            string input = Console.ReadLine() ?? "";

            Console.WriteLine(
                "\n" +
                $"입력하신 이름은 [{input}]입니다." +
                "\n");

            string name = input;

            // ===========================
            // 메뉴 리스트 설정
            switch (MenuUtil.OpenMenu("저장", "다시 입력","메인 메뉴로"))
            {
                case 0:
                    // 저장
                    StorageController.SetSaveName($"{name}");
                    SetPlayerJob();
                    break;
                case 1:
                    SetPlayerName();
                    break;
                case 2: 
                case -1: GameManager.EnterMain();  break;
            }
            // ===========================
        }

        static void SetPlayerJob()
        {
            Console.Clear();
            ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt", ConsoleColor.DarkMagenta, ConsoleColor.Green);
            Console.WriteLine();

            // 메뉴 리스트 설정
            var selections = new Dictionary<int, string>();

            int index = 0;
            foreach (JobName jobName in Enum.GetValues(typeof(JobName)))
            {
                if (jobName == JobName.None) continue;

                selections.Add(index++, GetJobNameToKor(jobName));
            }

            // ===========================
            // 메뉴 리스트 생성
            string[] jobNameList = new string[selections.Count];

            for (int i = 0; i < selections.Count; i++)
            {
                jobNameList[i] = selections[i];
            }

            int selection = MenuUtil.OpenMenu(jobNameList);

            if (selection < 0) { SetPlayerName(); }
            else
            {
                StorageController.SetSaveJob((JobName)selection);
                // 메뉴 씬으로 이동
                GameManager.StartGame(new());
            }
            // ===========================
        }
    }
}
