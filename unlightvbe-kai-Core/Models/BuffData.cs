using unlightvbe_kai_core.Models.Skill;

namespace unlightvbe_kai_core.Models
{
    public class BuffData()
    {
        public required BuffSkillModel Buff { get; init; }
        /// <summary>
        /// 數值
        /// </summary>
        public int Value { get; set; }
        /// <summary>
        /// 統計數(回合/累計)
        /// </summary>
        public int Total { get; set; }
    }
}
