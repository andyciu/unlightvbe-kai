using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum
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
}
