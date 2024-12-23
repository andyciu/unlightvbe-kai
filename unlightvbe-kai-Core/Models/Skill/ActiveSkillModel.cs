using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.Skill
{
    public class ActiveSkillModel : SkillModel<ActiveSkillDelegate>
    {
        /// <summary>
        /// 回合階段
        /// </summary>
        public required PhaseType Phase { get; init; }
        /// <summary>
        /// 距離
        /// </summary>
        public required List<CommandPlayerDistanceType> Distance { get; init; } = [];
        /// <summary>
        /// 卡片條件集合
        /// </summary>
        public required List<SkillCardConditionModel> Cards { get; init; } = [];
        /// <summary>
        /// 主動技能於角色待命狀態時仍可執行之啟動旗標
        /// </summary>
        public bool IsCallOnBackground { get; set; } = false;
    }
}
