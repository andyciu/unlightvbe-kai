using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models.UserInterface;

namespace unlightvbe_kai_core.Models.SkillArgs
{
    /// <summary>
    /// 技能引數資料傳遞(主動技能)
    /// </summary>
    public class ActiveSkillArgsModel : SkillArgsModelBase
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
        /// 技能指定發動條件(回合階段)
        /// </summary>
        public PhaseType SkillPhase { get; set; }
        /// <summary>
        /// 技能指定發動條件(距離)
        /// </summary>
        public List<CommandPlayerDistanceType> SkillDistance { get; set; } = [];
        /// <summary>
        /// 技能指定發動條件(卡片條件集合)
        /// </summary>
        public List<SkillCardConditionModel> SkillCardCondition { get; set; } = [];
        /// <summary>
        /// 發動之技能目前位置
        /// </summary>
        public int SkillIndex { get; set; }

        /// <summary>
        /// 檢查主動技能是否符合發動條件
        /// </summary>
        /// <returns></returns>
        public bool CheckActiveSkillCondition()
        {
            if (!SkillDistance.Any(x => x == PlayerDistance)) return false;
            if (SkillPhase != Phase) return false;
            List<int> tmpCardNumberEqual = [];
            List<int> tmpCardNumberCardConditionNone = [];
            List<SkillCardConditionModel> tmpConditionIndexForCardConditionNone = [];

            for (int i = 0; i < SkillCardCondition.Count; i++)
            {
                var cardCondition = SkillCardCondition[i];
                int tmptotalnum;
                switch (cardCondition.Scope)
                {
                    case SkillCardConditionScopeType.Above:
                        if (cardCondition.CardType == ActionCardType.None)
                        {
                            tmpConditionIndexForCardConditionNone.Add(cardCondition);
                        }
                        else if (!ActionCardTotal[(int)UserPlayerRelativeType.Self].TryGetValue(cardCondition.CardType, out tmptotalnum) ||
                            !(tmptotalnum >= cardCondition.Number))
                        {

                            return false;
                        }
                        break;
                    case SkillCardConditionScopeType.Below:
                        if (cardCondition.CardType == ActionCardType.None)
                        {
                            tmpConditionIndexForCardConditionNone.Add(cardCondition);
                        }
                        else if (!ActionCardTotal[(int)UserPlayerRelativeType.Self].TryGetValue(cardCondition.CardType, out tmptotalnum) ||
                            !(tmptotalnum <= cardCondition.Number))
                        {
                            return false;
                        }
                        break;
                    case SkillCardConditionScopeType.Equal:
                        bool tmpIsFind = false;
                        foreach (var card in CardDecks[CardDeckRelativeType.Play_Self])
                        {
                            if ((card.Value.UpperType == cardCondition.CardType || cardCondition.CardType == ActionCardType.None) &&
                                card.Value.UpperNum == cardCondition.Number && !tmpCardNumberEqual.Any(x => x == card.Key))
                            {
                                tmpCardNumberEqual.Add(card.Key);
                                tmpIsFind = true;
                                break;
                            }
                        }

                        if (!tmpIsFind) return false;

                        break;
                    case SkillCardConditionScopeType.None:
                        return true;
                }
            }

            foreach (var condition in tmpConditionIndexForCardConditionNone.OrderByDescending(x => x.Number).OrderBy(x => x.Scope))
            {
                bool tmpIsFind = false;
                foreach (var card in CardDecks[CardDeckRelativeType.Play_Self].OrderByDescending(x => x.Value.UpperNum))
                {
                    switch (condition.Scope)
                    {
                        case SkillCardConditionScopeType.Above:
                            if (card.Value.UpperNum >= condition.Number &&
                                !tmpCardNumberEqual.Any(x => x == card.Key) && !tmpCardNumberCardConditionNone.Any(x => x == card.Key))
                            {
                                tmpCardNumberCardConditionNone.Add(card.Key);
                                tmpIsFind = true;
                            }
                            break;
                        case SkillCardConditionScopeType.Below:
                            if (card.Value.UpperNum <= condition.Number &&
                                !tmpCardNumberEqual.Any(x => x == card.Key) && !tmpCardNumberCardConditionNone.Any(x => x == card.Key))
                            {
                                tmpCardNumberCardConditionNone.Add(card.Key);
                                tmpIsFind = true;
                            }
                            break;
                    }
                    if (tmpIsFind) break;
                }

                if (!tmpIsFind) return false;
            }

            return true;
        }

        /// <summary>
        /// 主動技能出牌時發動驗證一般檢查程序
        /// </summary>
        /// <param name="commandFormater"></param>
        public void CheckActiveSkillTurnOnOffStandardAction(SkillCommandModelFormatConverter commandFormater)
        {
            if (CharacterIsOnField)
            {
                var tmpCheck = CheckActiveSkillCondition();
                if (tmpCheck && !CharacterActiveSkillIsActivate[(int)UserPlayerRelativeType.Self][SkillIndex])
                {
                    commandFormater.SkillTurnOnOffWithLineLight(true);
                }
                else if (!tmpCheck && CharacterActiveSkillIsActivate[(int)UserPlayerRelativeType.Self][SkillIndex])
                {
                    commandFormater.SkillTurnOnOffWithLineLight(false);
                }
            }
        }
    }
}
