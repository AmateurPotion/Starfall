using Starfall.Contents.Binary;
using Starfall.IO;
using Starfall.IO.CUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Starfall.Core.CreatePlayer;

namespace Starfall.Core
{
    public class QuestData
    {
        public string Title { get; set; }
        public string Comment { get; set; }
        public string Reward { get; set; }
        public string Goal { get; set; }
    }

    public class QuestManager
    {
        public void ShowQuestList()
        {
    `           // 퀘스트 리스트 Linq 구문 이용해서 압축
            var questTitles = (from quest in GameManager.quests select quest.Value.Title).ToArray();

            int selection = MenuUtil.OpenMenu(questTitles);


        }

        public void ShowQuest()
        {
            foreach (var kvp in GameManager.quests)
            {
                var quest = kvp.Value;
                Console.WriteLine($"ID: {kvp.Key}");
                Console.WriteLine($"제목: {quest.Title}");
                Console.WriteLine($"설명: {quest.Comment}");
                Console.WriteLine($"보상: {quest.Reward}");
                Console.WriteLine($"목표: {quest.Goal}");
            }
        }
    }
}
