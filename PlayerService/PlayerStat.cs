namespace Starfall.PlayerService;

public partial class Player
{
  public float presentHp = 0;
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

  public float criticalDamageMultiplyer = 160f;  // 치명타가 뜨면 160% 데미지
  public float criticalChance = 15f;   // 기본 치명타 확률 15%
  public float evasionChance = 10f;   // 기본 회피 확률 10%
}