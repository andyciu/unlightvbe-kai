namespace unlightvbe_kai_core.Models.SkillArgs
{
    public class BuffArgsModel : SkillArgsModelBase
    {
        /// <summary>
        /// 異常狀態自身目前數值
        /// </summary>
        public int BuffValue { get; set; }
        /// <summary>
        /// 異常狀態自身目前統計數(回合/累計)
        /// </summary>
        public int BuffTotal { get; set; }
    }
}
