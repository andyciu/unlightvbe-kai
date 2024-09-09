using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_core.Models
{
    /// <summary>
    /// 技能引數資料傳遞(主動技能)
    /// </summary>
    public class ActiveSkillArgsModel
    {
        /// <summary>
        /// 雙方當前血量[VBEAtkingVSF]
        /// </summary>
        public int[][] CharacterHP { get; set; }
        /// <summary>
        /// 雙方血量最大值[VBEAtkingVSF]
        /// </summary>
        public int[][] CharacterHPMAX { get; set; }
        /// <summary>
        /// 目前角色之場上位置(VBEAtkingVSF順序)[VBEAtkingVSS]
        /// </summary>
        public int CharacterBattleIndex { get; set; }
        /// <summary>
        /// 雙方手牌數[VBEAtkingVSS]
        /// </summary>
        public int[] HoldCardCount { get; set; }
        /// <summary>
        /// 雙方出牌數[VBEAtkingVSS]
        /// </summary>
        public int[] PlayCardCount { get; set; }
        /// <summary>
        /// 雙方目前總骰數
        /// </summary>
        public int[] DiceTotal { get; set; }
        /// <summary>
        /// 雙方擲骰後正面數量
        /// </summary>
        public int[] DiceTrue { get; set; }
        /// <summary>
        /// 系統公骰擲骰後正骰數量差(攻擊正面骰減去防禦正面骰)
        /// </summary>
        public int DiceTrueTotal { get; set; }
        /// <summary>
        /// 當前距離
        /// </summary>
        public PlayerDistanceType PlayerDistance { get; set; }
        /// <summary>
        /// 當前回合數
        /// </summary>
        public int TurnNum { get; set; }
        /// <summary>
        /// 牌堆卡牌數
        /// </summary>
        public int DeckNum { get; set; }
        /// <summary>
        /// 目前階段
        /// </summary>
        public PhaseType Phase { get; set; }
        /// <summary>
        /// 本回合何方先攻
        /// </summary>
        public UserPlayerRelativeType? AttackPhaseFirst { get; set; }
        /// <summary>
        /// 雙方戰鬥總人數
        /// </summary>
        public int[] CharacterCount { get; set; }
        /// <summary>
        /// 雙方目前卡牌最大數量上限
        /// </summary>
        public int[] PlayerHoldMaxCount { get; set; }
        /// <summary>
        /// 雙方出牌種類及數值統計資料
        /// </summary>
        public Dictionary<ActionCardType, int>[] ActionCardTotal { get; set; }
        /// <summary>
        /// 卡牌集合(戰鬥系統場上卡牌資訊一覽)
        /// </summary>
        public Dictionary<CardDeckRelativeType, Dictionary<int, CardModel>> CardDecks { get; set; }
        /// <summary>
        /// 卡牌集合索引(卡牌編號->對應集合)
        /// </summary>
        public Dictionary<int, CardDeckRelativeType> CardDeckIndex { get; set; }
    }
}
