using Spectre.Console;
using Starfall.Contents;
using Starfall.Contents.Json;
using Starfall.IO.CUI;
using Starfall.PlayerService;

namespace Starfall.Core
{
    public class Battle
    {
        private Player player = new();
        private List<Monster> monsters = [];
        private Random random = new();
        private Dictionary<string, Skill> skills = GameManager.skills;

        // 플레이어 관련
        private bool isDead = false;
        private int resultGold = 0;
        private int resultExp = 0;

        // 몬스터 관련
        private float monCrit = 15f;
        private float monCritDmg = 160f;
        private float monEvasion = 10f;

        private Dictionary<string, int> buff = [];

        public void StartBattle(Player player, Dictionary<string, MonsterData> monstersD)
        {
            this.player = player;
            foreach(KeyValuePair<string, MonsterData> kv in monstersD)
            {
                this.monsters.Add(new Monster(kv.Value));
            }
            player.presentHp = player.TrueHp;

            while (true)
            {
                Console.Clear();
                AnsiConsole.MarkupLine("Battle!!\n");

                // 몬스터 목록 출력
                for (int i = 0; i < monsters.Count; i++)
                {
                    var m = monsters[i];
                    var hpText = m.IsAlive ? $"HP {m.Hp}" : "Dead";
                    AnsiConsole.MarkupLine($"{i + 1} Lv.{m.Level} {m.Name}  {hpText}");
                }

                AnsiConsole.MarkupLine($"\n[[내 정보]]\nLv.{player.level} {player.name} {player.job.GetJobNameToKor()}\nHP {player.presentHp}/{player.TrueHp}\n");

                // 플레이어가 죽으면 전투 종료
                if (isDead)
                {
                    AnsiConsole.MarkupLine("\n플레이어가 쓰러졌습니다... Game Over.\n");
                    MenuUtil.OpenMenu("확인");
                    break;
                }

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

            CallResultPage();

            player.hp = player.presentHp;
        }

        private bool PlayerTurn()
        {
            var actionChoice = MenuUtil.OpenMenu(
                "1. 일반 공격",
                "2. 스킬 선택",
                "0. 턴 종료");

            switch (actionChoice)
            {
                case 0: // 일반 공격
                    return NormalAttack();
                case 1: // 스킬 공격
                    return SelectSkill();
                default:
                    AnsiConsole.MarkupLine("턴을 종료합니다...");
                    return true;
            }
        }

        private void MonsterTurn()
        {
            foreach (var m in monsters)
            {
                if (!m.IsAlive) continue;

                MonsterAttackPlayer(m, player);
            }

            foreach(var (key, value) in buff)
            {
                buff[key]--;
                if (value == 0)
                {
                    skills[key].Action("Off", player, monsters);
                }
            }
        }

        private bool NormalAttack()
        {
            var options = monsters.Select(m => $"- {m.Name} 공격").ToList();
            options.Add("- 취소");

            var choice = MenuUtil.OpenMenu([.. options]);
            if (choice == monsters.Count || choice == -1)
            {
                return false;
            }

            var target = monsters[choice];
            if (!target.IsAlive)
            {
                AnsiConsole.MarkupLine($"이미 쓰러진 {target.Name}은 공격할 수 없습니다.\n");
                MenuUtil.OpenMenu("다음");
                return false;
            }

            // 회피 여부 계산
            bool isEvasion = 100 * random.NextDouble() < monEvasion;
            if (isEvasion)
            {
                AnsiConsole.MarkupLine($"{target.Name}이 공격을 회피했습니다!");
            }
            else
            {
                PlayerAttackMonster(player, target, 100);    
            }        

            return true;
        }

        private bool SelectSkill()
        {
            var skillKeys = skills.Keys.ToList();
            var options = skillKeys.Select(k => $"- {k}").ToList();

            options.Add("- 취소");

            var choice = MenuUtil.OpenMenu([.. options]);
            if (choice == skillKeys.Count || choice == -1)
            {
                return false;
            }

            Skill selectedskill = skills[skillKeys[choice]];

            if (selectedskill.targetAmount == 0)
            {
                    selectedskill.Action("Use", player, monsters);
                    if (selectedskill.durationTurn > 0)
                    {
                        buff.Add(selectedskill.name, selectedskill.durationTurn);
                    }
                    return true;
            }
            else
            {
                List<Monster> targetMonster = [];
                if (selectedskill.targetAmount >= monsters.Count)
                {
                    foreach (var m in monsters)
                    {
                        targetMonster.Add(m);
                    }
                    selectedskill.Action("Use", player, targetMonster);
                    return true;
                }
                else
                {
                    for (int i = 0; i < selectedskill.targetAmount; i++)
                    {
                        SkillAttack(selectedskill, targetMonster);
                    }
                    return true;
                }
            }
        }
        
        private void SkillAttack(Skill skill, List<Monster> monList)
        {
            while(true)
            {
                var options = monsters.Select(m => $"- {m.Name} 공격").ToList();

                var choice = MenuUtil.OpenMenu([.. options]);

                var target = monsters[choice];
                if (!target.IsAlive)
                {
                    AnsiConsole.MarkupLine($"이미 쓰러진 {target.Name}은 공격할 수 없습니다.\n");
                    MenuUtil.OpenMenu("다음");
                    continue;
                }

                monList.Add(target);

                skill.Action("Use", player, monList);

                break;
            }
        }

        private void PlayerAttackMonster(Player player, Monster monster, double multiplier)
        {
            AnsiConsole.MarkupLine("");

            double playerDamage = player.TrueAtk * (random.NextDouble()  * 0.2 + 0.9);  // +-10%의 오차 적용
            playerDamage *= multiplier / 100;  // 배율 적용

            // 치명타 여부 게산
            bool isCrit = 100 * random.NextDouble() < player.criticalChance;
            if (isCrit)
            {
                playerDamage *= player.criticalDamageMultiplyer / 100;
                AnsiConsole.Markup("치명타!!!! ");
            }

            playerDamage = Math.Ceiling(playerDamage); // 0의 자리에서 올림

            AnsiConsole.MarkupLine($"{player.name}(이)가 {monster.Name}에게 {playerDamage}의 데미지를 입혔습니다!!");
            monster.Hp -= (float)playerDamage;
            if (!monster.IsAlive)
            {
                monster.Dead();
                resultExp += monster.Level;
                resultGold += monster.RewardGold;
            }
        }

        private void MonsterAttackPlayer(Monster monster, Player player)
        {
            double monDamage = 0;

            // 회피 여부 계산
            bool isEvasion = 100 * random.NextDouble() < player.evasionChance;
            if (isEvasion)
            {
                AnsiConsole.MarkupLine($"{player.name}(이)가 공격을 회피했습니다!");
                AnsiConsole.MarkupLine($"{monster.Name}에게 {monDamage}의 데미지를 받았습니다!\n");
            }
            else
            {
                // 플레이어의 방어 여부 계산
                if (player.TrueDef >= 1000)
                {
                    AnsiConsole.MarkupLine($"{monster.Name}에게 {monDamage}의 데미지를 받았지만 완벽한 방어로 0의 피해를 받았습니다!\n");
                }
                else
                {
                    monDamage = monster.Atk;

                    // 치명타 여부 계산
                    bool isCrit = 100 * random.NextDouble() < monCrit;
                    if (isCrit)
                    {
                        monDamage *= monCritDmg / 100;
                        AnsiConsole.Markup("치명타!! ");
                    }

                    monDamage = Math.Ceiling(monDamage); // 0의 자리에서 올림 처리
                    double reduceDamage = monDamage - player.TrueDef;
                    double realDamage = Math.Max(0, reduceDamage);
                    AnsiConsole.MarkupLine($"{monster.Name}에게 {monDamage}의 데미지를 받았지만 {player.TrueDef}의 피해를 막아서 {realDamage}의 피해를 받았습니다!\n");
                    player.presentHp -= (float)monDamage;
                }
            }

            // 플레이어 사망
            if (player.presentHp <= 0)
            {
                isDead = true;
            }
        }

        private void CallResultPage()
        {
            player.Exp += resultExp;
            player.gold += resultGold;

            Console.Clear();
            AnsiConsole.MarkupLine("Battle!! - Result\n");
            AnsiConsole.MarkupLine(!isDead ? "Victory" : "You Lose");

            AnsiConsole.MarkupLine($"""

            Lv.{player.level} {player.name} {player.job.GetJobNameToKor()}
            HP {player.TrueHp} -> {player.presentHp}

            획득한 경험치 -> +{resultExp}
            획득한 골  드 -> +{resultGold}

            """);

            if (isDead)
            {
                MenuUtil.OpenMenu("시작화면으로");
                GameManager.EnterMain();
            }
            else
                MenuUtil.OpenMenu("다음");
        }        
    }
}