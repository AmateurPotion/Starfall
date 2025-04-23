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

namespace Starfall.Core.Quest
{
	public static class QuestManager
	{
		static QuestCondition condition = new QuestCondition() { Target = "Minion", RequiredCount = 5 };

		static Dictionary<string, QuestData> acceptableQuests = new Dictionary<string, QuestData>();
		static Dictionary<string, QuestData> clearedQuests = new Dictionary<string, QuestData>();

		public static void EnterQuestMenu()
		{
			ShowAcceptableQuests();
		}

		static void ShowAcceptableQuests()
		{
			// ===========================
			Console.Clear();
			ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt");
			Console.WriteLine();
			// ===========================

			AnsiConsole.MarkupLine($"""
            
                [퀘스트 목록]]

                """);

			acceptableQuests = GameManager.quests;

			var questTitles = acceptableQuests
					.Select(quest => quest.Value.State == QuestData.QuestState.Completed
							? $"{quest.Value.Title} (완료 가능)" : quest.Value.Title)
					.ToArray();

			int selection = MenuUtil.OpenMenu(questTitles);

			ShowSelectedQuest(acceptableQuests[questTitles[selection]]);
		}

		static void ShowSelectedQuest(QuestData questData)
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
			//switch (MenuUtil.OpenMenu("수락", "거절", "뒤로가기"))
			//{
			//    case 0:   
			//        break;
			//    case 1:
			//        break;
			//    case 2:
			//    case -1: GameManager.EnterMain(); break;
			//}
			// ===========================
		}
	}
}
