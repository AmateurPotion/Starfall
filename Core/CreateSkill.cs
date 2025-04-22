using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Starfall.Contents;

namespace Starfall.Core
{
    public class CreateSkill
    {
        public static Skill CreateAlpha()
        {
            return new Skill
            {
                Name = "알파 스트라이크",
                ManaCost = 10,
                Effect = new Dictionary<Skill.SkillEffectType, int>
                {
                    { Skill.SkillEffectType.SingleAttack, 3}
                }
            };
        }

        public static Skill CreateDouble()
        {
            return new Skill
            {
                Name = "더블 스트라이크",
                ManaCost = 15,
                Effect = new Dictionary<Skill.SkillEffectType, int>
                {
                    { Skill.SkillEffectType.MultipleAttack, 2}
                }
            };
        }

        public static Skill CreateShiled()
        {
            return new Skill
            {
                Name = "단단해지기",
                ManaCost = 10,
                Effect = new Dictionary<Skill.SkillEffectType, int>
                {
                    { Skill.SkillEffectType.Defense, 10}
                }
            };
        }

        public static Skill CreateHeal()
        {
            return new Skill
            {
                Name = "회복하기",
                ManaCost = 10,
                Effect = new Dictionary<Skill.SkillEffectType, int>
                {
                    { Skill.SkillEffectType.Heal, 2}
                }
            };
        }
    }
}
