using Spectre.Console;
using Starfall.Contents.Binary;
using Starfall.IO;
using Starfall.IO.CUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Starfall.Core.CreatePlayer;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Starfall.Core
{
    // 보상 설정 할 때 (예를 들어: 던전)
    // RewardType:이름or숫자
    // 예: Gold:100
    // 다수의 보상일 경우
    // 예: Gold:100,Item:낡은검,Item:무쇠갑옷
    public class RewardData
    {
        public enum RewardType
        {
            Gold,
            Item
        }

        Dictionary<RewardType, List<string>> rewards;

        public RewardData(string data) 
        {
            if (string.IsNullOrEmpty(data))
            {
                this.rewards = null;
                return;
            }

            this.rewards = new Dictionary<RewardType, List<string>>();
            string[] rewardDatas = data.Split(',');
            foreach (var reward in rewardDatas)
            {
                string[] detail = reward.Split(':');

                if (!Enum.TryParse(detail[0], out RewardType type))
                {
                    Console.WriteLine($"enum RewardType Error: {detail[0]}");
                    continue;
                }

                string value = detail[1];

                if (!this.rewards.ContainsKey(type))
                {
                    this.rewards.Add(type, new List<string>() { value });
                }
                else
                {
                    this.rewards[type].Add(value);
                }
            }
            return;
        }


    }

    public class QuestData
    {
        public QuestData() { }

        string title = string.Empty;
        string comment = string.Empty;
        RewardData rewardData = null;
        string goal = string.Empty;
        bool isCleared = false;

        #region Property
        public string Title
        {
            get { return title; }
        }
        public string Comment
        {
            get { return comment; }
        }
        public RewardData RewardData
        {
            get { return rewardData; }
        }
        public string Goal
        {
            get { return goal; }
        }
        public bool IsCleared
        {
            get { return isCleared; }
        }
        #endregion
    }

    public class QuestManager
    {
        Dictionary<string, QuestData> acceptableQuests = new Dictionary<string, QuestData>();
        Dictionary<string, QuestData> clearedQuests = new Dictionary<string, QuestData>();

        public void ShowAcceptableQuests()
        {
            // ===========================
            Console.Clear();
            ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt");
            Console.WriteLine();
            // ===========================

            AnsiConsole.MarkupLine($"""
            
                [퀘스트 목록]]

                """);

            this.acceptableQuests = GameManager.quests;

            var questTitles = acceptableQuests
                .Select(quest => quest.Value.IsCleared
                    ? $"{quest.Value.Title} (완료 가능)" : quest.Value.Title)
                .ToArray();

            int selection = MenuUtil.OpenMenu(questTitles);

            ShowSelectedQuest(this.acceptableQuests[questTitles[selection]]);
        }

        public void ShowSelectedQuest(QuestData questData)
        {
            AnsiConsole.MarkupLine($"""

                [#d1949e]{questData.Title}[/]

                {questData.Comment}

                - {questData.Goal}

                - 보상 -
                [#d1949e]{questData.RewardData}[/]


                """);
    
            // ===========================
            // 메뉴 리스트 설정
            //switch (MenuUtil.OpenMenu("저장", "다시 입력", "메인 메뉴로"))
            //{
            //    case 0:
            //        // 저장
            //        StorageController.SetSaveName($"{name}");
            //        var data = new GameData
            //        {
            //            Name = name
            //        };
            //        SetPlayerJob(data);
            //        break;

            //    case 1:
            //        SetPlayerName();
            //        break;

            //    case 2:
            //    case -1: GameManager.EnterMain(); break;
            //}
            // ===========================
        }
    }
}
