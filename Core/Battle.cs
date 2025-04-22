using System.Runtime.CompilerServices;
using Spectre.Console;
using Starfall.IO.CUI;
using Starfall.PlayerService;

namespace Starfall.Core
{
    // 몬스터 클래스 정의
    class Monster
    {
        public string name = "Monster";
        public int level = 1;
        public float hp = 100;
        public float atk = 0;
        public float def = 0;

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

    public class Battle()
    {
        private Player player = new();
        private List<Monster> monsters = [];

        private Random random = new();
        private double crit = 50;
        private double critDmg = 50;
        private double evasion = 20;
        private bool isDef = false;

        public void StartBattle(Player player)
        {
            this.player = player;
            this.monsters =
            [
                new("미니언", 2, 15, 5, 0),
                new("공허충", 3, 10, 9, 0),
                new("대포미니언", 5, 25, 8, 0)
            ];
            float maxHp = player.TrueHp;

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

                AnsiConsole.MarkupLine($"\n내 정보\nLv.{player.level} {player.name} {player.job.GetJobNameToKor()}\nHP {player.hp}/{maxHp}\n");

                // 모든 몬스터가 죽었으면 전투 종료
                if (monsters.All(m => !m.IsAlive))
                {
                    AnsiConsole.MarkupLine("모든 몬스터를 처치했습니다! 전투 종료.\n");
                    MenuUtil.OpenMenu("확인");
                    break;
                }

                // 공격할 대상 선택
                if (!PlayerTurn()) continue;

                AnsiConsole.MarkupLine("\n====================================\n");

                MonsterTurn();

                MenuUtil.OpenMenu("다음");
            }
        }

        private bool PlayerTurn()
        {
            if (isDef)
            {
                isDef = false;
                player.def -= 1000;
            }
            
            var actionChoice = MenuUtil.OpenMenu(
                "1. 일반 공격",
                "2. 스킬 공격",
                "3. 방어하기",
                "0. 턴 종료");

            switch (actionChoice)
            {
                case 0: // 일반 공격
                    return SelectTarget(1.0);  // 100% 배율
                case 1: // 스킬 공격
                    return SelectTarget(2.0);  // 200% 배율
                case 2: // 방어하기
                    isDef = true;
                    AnsiConsole.MarkupLine($"{player.name}(이)가 방어 자세를 취합니다!\n");
                    player.def += 1000;
                    MenuUtil.OpenMenu("다음");
                    return true;
                case 3: // 턴 종료
                default:
                    AnsiConsole.MarkupLine("턴을 종료합니다...\n");
                    return true;
            }
        }

        private void MonsterTurn()
        {
            foreach (Monster m in monsters)
            {
                if (!m.IsAlive) continue;

                MonsterAttackPlayer(m, player);
            }
        }

        private bool SelectTarget(double multiplier)
        {
            var options = monsters.Select(m => $"- {m.name} 공격").ToList();
            options.Add("- 취소");

            var choice = MenuUtil.OpenMenu([.. options]);
            if (choice == monsters.Count || choice == -1)
            {
                AnsiConsole.MarkupLine("턴을 종료합니다...\n");
                return false;
            }

            var target = monsters[choice];
            if (!target.IsAlive)
            {
                AnsiConsole.MarkupLine($"이미 쓰러진 {target.name}은 공격할 수 없습니다.\n");
                MenuUtil.OpenMenu("다음");
                return false;
            }

            PlayerAttackMonster(player, target, multiplier);

            return true;
        }

        private void PlayerAttackMonster(Player player, Monster monster, double multiplier)
        {
            bool isCrit = 100 * random.NextDouble() < crit;
            bool isEvasion = 100 * random.NextDouble() < evasion;
            double playerDamage;

            if (!isEvasion)
            {
                playerDamage = isCrit ? player.TrueAtk * (1 + critDmg / 100) : player.TrueAtk;
                playerDamage *= multiplier;
            }
            else
            {
                playerDamage = 0;
                AnsiConsole.MarkupLine($"{monster.name}이 공격을 회피했습니다!");
            }

            AnsiConsole.MarkupLine($"\n{player.name}(이)가 {monster.name}에게 {playerDamage}의 데미지를 입혔습니다!");
            monster.hp -= (float)playerDamage;
            if (!monster.IsAlive)
                AnsiConsole.MarkupLine($"{monster.name}을(를) 처치했습니다!");
        }

        private void MonsterAttackPlayer(Monster monster, Player player)
        {
            bool isCrit = 100 * random.NextDouble() < crit;
            bool isEvasion = 100 * random.NextDouble() < evasion;
            double baseDamage = 0;

            if (!isEvasion)
            {
                baseDamage = isCrit ? monster.atk * (1 + critDmg / 100) : monster.atk;
                double reduceDamage = baseDamage - player.TrueDef;
                double realDamage = Math.Max(0, reduceDamage);
                if (player.TrueDef >= 1000)
                {
                    AnsiConsole.MarkupLine($"{monster.name}에게 {baseDamage}의 데미지를 받았지만 완벽한 방어로 0의 피해를 받았습니다!\n");
                }
                else
                {
                    AnsiConsole.MarkupLine($"{monster.name}에게 {baseDamage}의 데미지를 받았지만 {player.TrueDef}의 피해를 막아서 {realDamage}의 피해를 받았습니다!\n");

                }
                player.hp -= (float)realDamage;
            }
            else
            {
                AnsiConsole.MarkupLine($"{player.name}(이)가 공격을 회피했습니다!");
                AnsiConsole.MarkupLine($"{monster.name}에게 {baseDamage}의 데미지를 받았습니다!\n");
            }

            if (player.hp <= 0)
            {
                PlayerDead();
            }
        }

        private void PlayerDead()
        {
            AnsiConsole.MarkupLine("\n플레이어가 쓰러졌습니다... Game Over.\n");
            MenuUtil.OpenMenu("확인");
            GameManager.EnterMain();
        }
    }
}