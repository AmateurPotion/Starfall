using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Core.Quest
{
	public class QuestCondition
	{
		public string Target { get; set; }  // 예: "Minion"
		public int RequiredCount { get; set; }  // 예: 5
		public int CurrentCount { get; set; }  // 예: 2 (현재 진행 상황)
	}
}
