using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models.UserInterface;

namespace unlightvbe_kai_core.Models.SkillArgs
{
    /// <summary>
    /// 技能引數資料傳遞(被動技能)
    /// </summary>
    public class PassiveSkillArgsModel : SkillArgsModelBase
    {
        /// <summary>
        /// 目前角色主動技能是否啟動標記
        /// </summary>
        public bool[][] CharacterActiveSkillIsActivate { get; set; } = [];
        /// <summary>
        /// 目前角色被動技能是否啟動標記
        /// </summary>
        public bool[][] CharacterPassiveSkillIsActivate { get; set; } = [];
        /// <summary>
        /// 目前角色主動技能啟動次數
        /// </summary>
        public int[][] CharacterActiveSkillTurnOnCount { get; set; } = [];
        /// <summary>
        /// 目前角色被動技能啟動次數
        /// </summary>
        public int[][] CharacterPassiveSkillTurnOnCount { get; set; } = [];
        /// <summary>
        /// 卡牌集合(戰鬥系統場上卡牌資訊一覽)
        /// </summary>
        public Dictionary<CardDeckRelativeType, Dictionary<int, CardModel>> CardDecks { get; set; } = [];
        /// <summary>
        /// 卡牌集合索引(卡牌編號->對應集合)
        /// </summary>
        public Dictionary<int, CardDeckRelativeType> CardDeckIndex { get; set; } = [];
        /// <summary>
        /// 發動之技能目前位置
        /// </summary>
        public int SkillIndex { get; set; }
    }
}
