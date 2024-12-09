using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models
{
    public class SkillCommandProxyExecueCommandDataModel
    {
        /// <summary>
        /// 執行階段號
        /// </summary>
        public int StageNum { get; set; }
        /// <summary>
        /// 發動方
        /// </summary>
        public UserPlayerType Player { get; set; }
        /// <summary>
        /// 發動技能體系
        /// </summary>
        public SkillType SkillType { get; set; }
        /// <summary>
        /// 發動之技能目前位置
        /// </summary>
        public int SkillIndex { get; set; }
        /// <summary>
        /// 發動之技能唯一識別碼
        /// </summary>
        public string SkillID { get; set; } = string.Empty;
        /// <summary>
        /// 執行階段型態
        /// </summary>
        public SkillStageType StageType { get; set; }
        /// <summary>
        /// 目前角色之場上位置
        /// </summary>
        public int CharacterBattleIndex { get; set; }
        /// <summary>
        /// 執行階段是否為驗證模式
        /// </summary>
        public bool IsAuthMode { get; set; }
    }
}
