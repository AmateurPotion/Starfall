using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starfall.PlayerService;
using Starfall.Core;

namespace Starfall.Contents
{
    public class Skill
    {
        public enum SkillEffectType
        {
            None = 0,
            SingleAttack = 1,
            MultipleAttack = 2,
            Defense = 3,
            Heal = 4,
        }

				public string Name { get; set; }
				public int ManaCost { get; set; }

				public SkillType SkillType { get; set; }

	}

	public struct SkillType() {
		public Skill.SkillEffectType skillEffectType { get; set; }
		public float effectValue { get; set; }
		public TargetType target { get; set; }
	}


	public static class SkillEffectType
    {
        public static string GetSkillEffectTypeToKor(this Skill.SkillEffectType skillType) => skillType switch
        {
            Skill.SkillEffectType.None => "에러",
            Skill.SkillEffectType.SingleAttack => "피해",
            Skill.SkillEffectType.MultipleAttack => "피해",
            Skill.SkillEffectType.Defense => "쉴드",
            Skill.SkillEffectType.Heal => "회복",
            _ => "",
     };
	}

	public enum TargetType
	{
		Self = 0,
		SingleEnemy = 1,
		MultiEnemy = 2
	}
}
