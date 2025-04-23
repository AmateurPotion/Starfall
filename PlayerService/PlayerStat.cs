namespace Starfall.PlayerService;

public partial class Player
{
  public int level = 1;
  private int exp = 0;
  public int Exp
  {
    get => exp;
    set
    {
      exp += value;  // 누적 경험치 추가

      while (exp >= MaxExp)
      {
        exp -= MaxExp;
        level++;
      }
    }
  }
  // 요구하는 최대 경험치
  public int MaxExp => (int)(2.5 * Math.Pow(level, 2) + 17.5 * level - 10);

  public float criticalDamageMultiplyer = 1.5f;
  public float criticalChance = 0.3f;
  public float evasionChance = 0.8f;
}