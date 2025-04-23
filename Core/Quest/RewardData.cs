namespace Starfall.Core.Quest
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
				rewards = null;
				return;
			}

			rewards = new Dictionary<RewardType, List<string>>();
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

				if (!rewards.ContainsKey(type))
				{
					rewards.Add(type, new List<string>() { value });
				}
				else
				{
					rewards[type].Add(value);
				}
			}
			return;
		}

		public Dictionary<RewardType, List<string>> GetAll()
		{
			return rewards ?? new Dictionary<RewardType, List<string>>();
		}
	}
}
