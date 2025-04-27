namespace Starfall.PlayerService;

public enum Job
{
  None = 0,
  Warrior = 1,
  Wizard = 2,
  Rogue = 3,
}

public static class JobUtil
{
  public static string GetName(this Job className) => className switch
  {
    Job.None => "없음",
    Job.Warrior => "전사",
    Job.Wizard => "마법사",
    Job.Rogue => "도적",
    _ => "",
  };

  public static string GetDescription(this Job className) => className switch
  {
    Job.None => "없음",
    Job.Warrior => "단단해지기 스킬을 가지고 시작합니다.",
    Job.Wizard => "자가 회복 스킬을 가지고 시작합니다.",
    Job.Rogue => "알파 스트라이크 스킬을 가지고 시작합니다.",
    _ => "",
  };

  public static string GetBaseSkillId(this Job className) => className switch
  {
    Job.None => "없음",
    Job.Warrior => "단단해지기",
    Job.Wizard => "자가 회복",
    Job.Rogue => "알파 스트라이크",
    _ => "",
  };
}