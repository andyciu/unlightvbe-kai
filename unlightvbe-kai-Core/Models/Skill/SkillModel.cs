namespace unlightvbe_kai_core.Models.Skill
{
    public class SkillModel<T> where T : class
    {
        /// <summary>
        /// 技能名稱
        /// </summary>
        public string Name { get; init; } = string.Empty;
        /// <summary>
        /// 技能唯一識別碼
        /// </summary>
        public required string Identifier { get; init; }
        /// <summary>
        /// 欲使用之執行階段號
        /// </summary>
        public required List<int> StageNumber { get; init; }
        /// <summary>
        /// 技能委派物件
        /// </summary>
        public required T Function { get; init; }
    }
}
