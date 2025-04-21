using Spectre.Console;
using Starfall.Core.Classic;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.IO.Dataset;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        static void SetPlayerName()
        {
            // ===========================
            Console.Clear();
            ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt", ConsoleColor.DarkMagenta, ConsoleColor.Green);
            Console.WriteLine();
            // ===========================

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
                    //Console.WriteLine($"{name}");
                    StorageController.SetSaveName($"{name}");
                    GameData data = new GameData();
                    data.Name = name; 
                    SetPlayerJob(data);
                    break;
                case 1:
                    SetPlayerName();
                    break;
                case 2: 
                case -1: GameManager.EnterMain();  break;
            }
            // ===========================
        }

        static void SetPlayerJob(GameData data)
        {
            // ===========================
            //Console.Clear();
            ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt", ConsoleColor.DarkMagenta, ConsoleColor.Green);
            Console.WriteLine();
            // ===========================

            // 메뉴 리스트 설정
            var selections = new List<string>();

            foreach (JobName jobName in Enum.GetValues(typeof(JobName)))
            {
                if (jobName == JobName.None) continue;

                selections.Add(jobName.GetJobNameToKor());
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
                data.Job = (JobName)selection;
                // 메뉴 씬으로 이동
                GameManager.StartGame(data);
            }
            // ===========================
        }
    }
}
