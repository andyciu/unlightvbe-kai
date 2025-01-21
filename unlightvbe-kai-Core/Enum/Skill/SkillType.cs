using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Enum.Skill
{
    /// <summary>
    /// 技能體系類型
    /// </summary>
    public enum SkillType
    {
        /// <summary>
        /// 主動技能
        /// </summary>
        ActiveSkill = 1,
        /// <summary>
        /// 被動技能
        /// </summary>
        PassiveSkill,
        /// <summary>
        /// 異常狀態
        /// </summary>
        Buff,
        /// <summary>
        /// 人物實際狀態
        /// </summary>
        CharacterActualStatus
    }

    static class SkillTypeMethods
    {
        public static TriggerSkillType ToTriggerSkillType(this SkillType type)
        {
            return type switch
            {
                SkillType.ActiveSkill => TriggerSkillType.ActiveSkill,
                SkillType.PassiveSkill => TriggerSkillType.PassiveSkill,
                SkillType.Buff => TriggerSkillType.Buff,
                SkillType.CharacterActualStatus => TriggerSkillType.CharacterActualStatus,
                _ => throw new NotImplementedException()
            };
        }
    }
}
