namespace Starfall.Core.Quest
{
	public enum QuestType
	{
		Kill,
		Equip,
		LevelUp,
		StatUp
	}

	static class QuestTypeClass
	{
		public static string GetTypeToKor(QuestType type) => type switch
		{
			QuestType.Kill => "처치",
			QuestType.Equip => "장착",
			QuestType.LevelUp => "레벨 업",
			QuestType.StatUp => "스탯 업",
			_ => "",
		};
	}
}
