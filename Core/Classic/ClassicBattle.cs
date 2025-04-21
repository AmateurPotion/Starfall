using System.Text;
using Spectre.Console;
using Starfall.IO;
using Starfall.IO.CUI;
using Starfall.IO.Dataset;
using Starfall.PlayerService;

namespace Starfall.Core.Classic;
// 몬스터 클래스 정의
class Monster
{
    public string name = "Monster";
    public int level = 1;
    public float hp = 100;
    public float atk = 0;
    public float def = 5;

    public Monster(string name, int level, float hp, float atk, float def)
    {
        this.name = name;
        this.level = level;
        this.atk = atk;
        this.def = def;
        this.hp = hp;
    }

    public bool IsAlive => hp > 0;
    public void Dead() => Console.WriteLine($"{name}이 죽었습니다");
}

public class ClassicBattle()
{
    public void StartBattle(Player player)
    {
        // 세 마리 몬스터 생성
        var monsters = new List<Monster>
        {
            new("미니언", 2, 15, 5, 0),
            new("공허충", 3, 10, 9, 0),
            new("대포미니언", 5, 25, 8, 0)
        };

        while (true)
        {
            Console.Clear();
            AnsiConsole.MarkupLine("Battle!!\n");

            // 몬스터 목록 출력
            for (int i = 0; i < monsters.Count; i++)
            {
                var m = monsters[i];
                var hpText = m.IsAlive ? $"HP {m.hp}" : "Dead";
                AnsiConsole.MarkupLine($"{i + 1} Lv.{m.level} {m.name}  {hpText}");
            }

            AnsiConsole.MarkupLine($"\n내 정보\nLv.{player.level} {player.name} {player.job.GetJobNameToKor()}\nHP {player.TrueHp}/{player.hp}\n");

            // 모든 몬스터가 죽었으면 전투 종료
            if (monsters.All(m => !m.IsAlive))
            {
                AnsiConsole.MarkupLine("\n모든 몬스터를 처치했습니다! 전투 종료.");
                MenuUtil.OpenMenu("확인");
                break;
            }

            // 공격할 대상 선택
            var options = new List<string>();
            foreach (var m in monsters)
                options.Add($"{m.name} 공격");

            options.Add("0. 나가기");

            var choice = MenuUtil.OpenMenu([.. options]);

            var target = monsters[choice];

            if (!target.IsAlive)
            {
                AnsiConsole.MarkupLine($"\n이미 쓰러진 {target.name}은 공격할 수 없습니다.");
            }
            else
            {
                target.hp -= player.TrueAtk;
                AnsiConsole.MarkupLine($"\n{target.name}에게 {player.TrueAtk}의 데미지를 입혔습니다!");
                if (!target.IsAlive)
                    AnsiConsole.MarkupLine($"{target.name}을 처치했습니다!");
            }

            MenuUtil.OpenMenu("다음");
        }
    }
}