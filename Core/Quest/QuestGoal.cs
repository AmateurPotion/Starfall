namespace Starfall.Core.Quest
{
	public class QuestGoal
	{
		public QuestType Type { get; private set; }
		public string Target { get; private set; }
		public int Amount { get; private set; }

		string questType;

		public QuestGoal(string goalStr)
		{
			// 퀘스트 설정 할 때 (예를 들어: 던전)
			// QuestType:이름*개수 or QuestType:이름
			// 예: "Kill:Minion*5" or Equip:낡은검
			var parts = goalStr.Split(':');
			Type = Enum.Parse<QuestType>(parts[0]);

			var targetAmount = parts[1].Split('*');
			Target = targetAmount[0];
			Amount = targetAmount.Length > 1 ? int.Parse(targetAmount[1]) : -1;
		}

		// 예: 미니언(target) 5(Amount)마리 처치(type) (2(condition.CurrentCount)/5(amount))
		public string GoalToString(int curCount)
		{
			string goalTxt = $"{Target} {GetAmountToString()} {QuestTypeClass.GetTypeToKor(Type)} ({curCount}/{Amount})";

			return goalTxt;
		}

		public string GoalToString()
		{
			string goalTxt = $"{Target} {GetAmountToString()} {QuestTypeClass.GetTypeToKor(Type)}";

			return goalTxt;
		}

		public string GetAmountToString()
		{
			if (Amount != -1)
			{
				return $"{Amount}{GetAmountUnit()}";
			}
			else
				return "";
		}

		// 표시 단위
		string GetAmountUnit()
		{
			return Type switch
			{
				QuestType.Kill => "마리",
				QuestType.LevelUp or
				QuestType.StatUp => "만큼",
				QuestType.Equip or    
				_ => ""                           
			};
		}
	}
}
