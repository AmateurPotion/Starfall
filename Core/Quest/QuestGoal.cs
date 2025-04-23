namespace Starfall.Core.Quest
{
	public class QuestGoal
	{
		public QuestType Type { get; private set; }
		public string Target { get; private set; }
		public int Amount { get; private set; }

		public QuestGoal(string goalStr)
		{
			// 퀘스트 설정 할 때 (예를 들어: 던전)
			// QuestType:이름*개수 or QuestType:이름
			// 예: "Kill:Minion*5" or Equip:낡은검
			var parts = goalStr.Split(':');
			Type = Enum.Parse<QuestType>(parts[0]);

			var targetAmount = parts[1].Split('*');
			Target = targetAmount[0];
			Amount = targetAmount.Length > 1 ? int.Parse(targetAmount[1]) : 1;
		}
	}

}
