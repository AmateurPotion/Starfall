using Spectre.Console;
using Starfall.Contents.Json;
namespace Starfall.Contents;

public class Monster(MonsterData data)
{
    public string name = data.Name;
    public float hp = data.Hp;
    public float atk = data.Atk;
    public float def = data.Def;
    public int level = data.Level;
    public int rewardGold = data.RewardGold;
    public float criticalChance = data.CriticalChance;
    public float criticalDamageMultiplyer = data.CriticalDamageMultiplyer;
    public float evasionChance = data.EvasionChance;

    public bool IsAlive => hp > 0;
    public void Dead()
    {
        AnsiConsole.MarkupLine($"{name}을(를) 처치했습니다!");
    }

    public Monster Duplicate()
        => new(data);
}