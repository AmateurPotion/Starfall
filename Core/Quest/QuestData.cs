using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Core.Quest
{
	public class QuestData
	{
		public enum QuestState
		{
			NotAccepted,
			InProgress,
			Completed
		}

		public QuestData() { }

		string title = string.Empty;
		string comment = string.Empty;
		RewardData rewardData = null;
		string goal = string.Empty;
		QuestState state = QuestState.NotAccepted;

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
		public QuestState State
		{
			get { return state; }
		}
		#endregion
	}

}
