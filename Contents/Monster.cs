using Spectre.Console;
using Starfall.Contents.Json;
namespace Starfall.Contents;

class Monster(MonsterData data)
{
    public string Name { get; private set; } = data.Name;
    public float Hp { get; set; } = data.Hp;
    public float Atk { get; private set; } = data.Atk;
    public float Def { get; private set; } = data.Def;
    public int Level { get; private set; } = data.Level;
    public int RewardGold { get; private set; } = data.RewardGold;

    public bool IsAlive => Hp > 0;
    public void Dead()
    {
        AnsiConsole.MarkupLine($"{Name}을(를) 처치했습니다!");
    }
}