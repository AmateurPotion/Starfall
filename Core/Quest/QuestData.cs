namespace Starfall.Core.Quest
{
	public class QuestData
	{
		public enum QuestState
		{
			NotAccepted,
			InProgress,
			Completed,
			GotReward
		}

		public string Title { get; private set; }
		public string Comment { get; private set; }
		public string GoalRaw { get; private set; }
		public QuestGoal Goal { get; private set; }
		public RewardData RewardData { get; private set; }
		public QuestState State { get; set; } = QuestState.NotAccepted;

		public QuestData(string title, string comment, string goalRaw, string rewardRaw)
		{
			Title = title;
			Comment = comment;
			GoalRaw = goalRaw;
			Goal = new QuestGoal(goalRaw);
			RewardData = new RewardData(rewardRaw);
		}

		public static QuestData FromJson(QuestJson json)
		{
			var data = new QuestData(json.Title, json.Comment, json.Goal, json.Reward);

			if (Enum.TryParse(json.State, out QuestState parsedState))
			{
				data.State = parsedState;
			}

			return data;
		}
	}
}
