using Starfall.PlayerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Starfall.Core.Quest
{
	public static class RewardSystem
	{
		public static void ShowRewardScreen()
		{
			GameManager.EnterMain();
		}

		public static void GiveReward(Player player, RewardData rewardData)
		{
			if (rewardData == null)
			{
				Console.WriteLine("보상 데이터가 없습니다.");
				return;
			}

			foreach (var reward in rewardData.GetAll())
			{
				switch (reward.Key)
				{
					case RewardData.RewardType.Gold:
						foreach (var amountStr in reward.Value)
						{
							if (int.TryParse(amountStr, out int amount))
							{
								player.gold += amount;
								Console.WriteLine($"[Gold +{amount}] 골드를 받았습니다.");
							}
						}
						break;

					case RewardData.RewardType.Item:
						foreach (var itemName in reward.Value)
						{
							if (GameManager.items.TryGetValue(itemName, out var item))
							{
								if (player.inventory.ContainsKey(item))
									player.inventory[item]++;
								else
									player.inventory[item] = 1;

								Console.WriteLine($"[Item] {itemName}을(를) 획득했습니다.");
							}
							else
							{
								Console.WriteLine($"[Warning] 아이템 '{itemName}'은 존재하지 않습니다.");
							}
						}
						break;
				}
			}
		}

	}
}
