using Spectre.Console;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.PlayerService;
using System.Linq;
using System.Numerics;
using static Starfall.Core.Quest.QuestData;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Starfall.Core.Quest
{
	public static class QuestManager
	{
		static Dictionary<string, QuestCondition> questConditions = new Dictionary<string, QuestCondition>();

		static Dictionary<string, QuestData> acceptableQuests = new Dictionary<string, QuestData>();
		public static Dictionary<string, QuestData> inProgressQuests = new Dictionary<string, QuestData>();
		static Dictionary<string, QuestData> clearedQuests = new Dictionary<string, QuestData>();

		static Player player => GameManager.Instance!.player;

		public static void EnterQuestMenu()
		{
			// ===========================
			Console.Clear();
			ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt");
			Console.WriteLine();
			// ===========================

			AnsiConsole.MarkupLine($"""
            
                [[퀘스트 목록]]

                """);


			var notAccepted = new Dictionary<string, QuestData>();

			// 상태에 따라 분류
			if (GameManager.quests.Count > 0)
			{
				notAccepted = GameManager.quests
					.Where(q => q.Value.State == QuestState.NotAccepted)
					.ToDictionary(q => q.Key, q => q.Value);

				acceptableQuests = notAccepted;
			}
			else { Console.WriteLine("퀘스트 목록이 비어있음"); }

			var completed = new Dictionary<string, QuestData>();
			var merged = new Dictionary<string, QuestData>();


			if (inProgressQuests.Count > 0)
			{
				// Update All Quest Progress
				foreach (var quest in inProgressQuests)
				{
					UpdateQuestProgress(quest.Key);
				}

				merged = inProgressQuests
					.Concat(acceptableQuests)
					.ToDictionary(q => q.Key, q => q.Value);
			}
			else
			{
				merged = acceptableQuests;
			}

			// Menu에 넘겨줄 Title배열	
			// 퀘스트를 완료할 수 있다면 (완료 가능)을 붙임
			string[] questTitles = merged
				.Select(q =>
					q.Value.State == QuestState.Completed
						? $"{q.Value.Title} (완료 가능)"
					: q.Value.State == QuestState.InProgress
						? $"{q.Value.Title} (진행중)"
					: q.Value.Title
				)
				.Concat(new[] { "뒤로가기" })
				.ToArray();


			int selection = MenuUtil.OpenMenu(questTitles);

			// 메뉴 씬으로
			if (selection == questTitles.Length - 1)
			{
				if (GameManager.Instance != null)
					GameManager.Instance.Start();
				else
					Console.WriteLine("Game.cs Instance 없음");
			}
			else
			{
				// 선택한 퀘스트 보여주기
				string selectedKey = merged.Keys.ElementAt(selection);
				ShowSelectedQuest(selectedKey, merged[selectedKey]);
			}
		}

		static void ShowSelectedQuest(string key, QuestData questData)
		{
			// ===========================
			Console.Clear();
			ConsoleUtil.PrintTextFile("Starfall.Resources.intro.txt");
			Console.WriteLine();
			// ===========================

			string questGoal = questConditions.TryGetValue(key, out var condition)
							 ? $"{questData.Goal.GoalToString(condition.CurrentCount)}"
							 : $"{questData.Goal.GoalToString()}";

			string reward = questData.RewardData.RewardToString();

			AnsiConsole.MarkupLine($"""

                [#d1949e]{questData.Title}[/]

                {questData.Comment}

                - 목표: {questGoal}

                - 보상 -
                [#d1949e]{reward}[/]


                """);

			// ===========================
			// 메뉴 리스트 설정
			string action = questData.State switch
			{
				QuestState.NotAccepted => "수락",
				QuestState.Completed => "보상 받기",
				_ => "확인"
			};

			int selection = MenuUtil.OpenMenu(0, false, action, "뒤로가기");

			if (selection == 0)
			{
				switch (questData.State)
				{
					case QuestState.NotAccepted:
						AcceptQuest(key, questData);
						break;
					case QuestState.Completed:
						CompleteQuest(key);
						break;
				}
			}

			EnterQuestMenu();
		}

		// 퀘스트 수락
		public static void AcceptQuest(string key, QuestData data)
		{
			inProgressQuests.Add(key, data);
			acceptableQuests.Remove(key);

			if (data.Goal.Type is QuestType.Kill or QuestType.StatUp)
			{
				questConditions[key] = new QuestCondition
				{
					Target = data.Goal.Target,
					CurrentCount = 0
				};
			}

			data.State = QuestState.InProgress;
		}

		// 퀘스트 완료
		public static void CompleteQuest(string key)
		{
			if (!GameManager.quests.TryGetValue(key, out var quest)) return;
			if (quest.State != QuestState.Completed) return;

			clearedQuests.Add(key, inProgressQuests[key]);
			inProgressQuests.Remove(key);

			// 보상 지급
			RewardSystem.GiveReward(player, quest.RewardData);
			quest.State = QuestState.GotReward;
		}


		// 던전 끝났을 때 불러오기
		// Kill 진행 상태 업데이트
		public static void UpdateQuestProgress(string key, int amount)
		{
			if (!inProgressQuests.TryGetValue(key, out var quest)) return;

			var goal = quest.Goal;

			if (goal.Type is QuestType.Kill)
			{
				if (!questConditions.TryGetValue(key, out var condition)) return;

				condition.CurrentCount += amount;

				if (IsGoalComplete(goal, condition))
					quest.State = QuestState.Completed;
			}
			else
			{
				// 다른 타입은 이쪽으로 던짐
				UpdateQuestProgress(key);
			}
		}

		// 퀘스트 목록 생성할 때 불러오기
		// Equip, LevelUp, StatUp 진행 상태 체크
		public static void UpdateQuestProgress(string key)
		{
			if (!inProgressQuests.TryGetValue(key, out var quest)) return;

			if (IsGoalComplete(quest.Goal, QuestCondition.NULL))
				quest.State = QuestState.Completed;
		}

		// 목표를 달성했는가?
		public static bool IsGoalComplete(QuestGoal goal, QuestCondition condition)
		{
			if (condition == QuestCondition.NULL) return false;

			return goal.Type switch
			{
				QuestType.Kill => condition != null && condition.Target == goal.Target && condition.CurrentCount >= goal.Amount,
				QuestType.Equip => IsItemEquipped(player, goal.Target),
				QuestType.LevelUp => player?.level >= goal.Amount,
				QuestType.StatUp => player?.GetStatAsString(goal.Target) >= goal.Amount,
				_ => false
			};
		}

		private static bool IsItemEquipped(Player player, string itemName)
		{
			return GameManager.items.TryGetValue(itemName, out var item)
					&& player.inventory.TryGetValue(item, out var equip)
					&& equip == 1;
		}

	}
}
