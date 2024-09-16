namespace unlightvbe_kai_core.Enum
{
    /// <summary>
    /// 執行指令列舉
    /// </summary>
    public enum SkillCommandType
    {
        None = 0,
        /// <summary>
        /// 人物必殺技狀態燈控制(自身)
        /// </summary>
        SkillLineLight,
        /// <summary>
        /// 人物必殺技啟動碼控制(自身)
        /// </summary>
        SkillTurnOnOff,
        /// <summary>
        /// 人物必殺技狀態燈控制(其他)
        /// </summary>
        SkillLineLightAnother,
        /// <summary>
        /// 人物必殺技啟動碼控制(其他)
        /// </summary>
        SkillTurnOnOffAnother,
        /// <summary>
        /// 技能動畫圖像執行
        /// </summary>
        SkillAnimateStartPlay,
        BattleTurnControl,
        BattleSendMessage,
        BattleStartDice,
        AtkingInformationRecord,
        EventPlayerAllActionOff,
        EventActiveAIScore,
        BattleMoveControl,
        EventMoveActionOff,
        PersonMoveControl,
        PersonMoveActionChange,
        PersonAttackFirstControl,
        /// <summary>
        /// 攻擊/防禦階段系統骰數變化量控制
        /// </summary>
        EventTotalDiceChange,
        EventPersonAbilityDiceChange,
        PersonTotalDiceControl,
        AtkingTrueDiceControl,
        PersonBloodControl,
        EventBloodActionOff,
        EventBloodActionChange,
        EventBloodReflection,
        EventHPLActionOff,
        EventHPLActionChange,
        EventHPLReflection,
        PersonAtkingOff,
        PersonPassiveOff,
        PersonAtkingOffSelect,
        PersonPassiveOffSelect,
        PersonAtkingInvalid,
        PersonResurrect,
        EventPersonResurrectActionOff,
        PersonChangeBattleImage,
        AtkingSeizeEnemyCards,
        EventAtkingSeizeEnemyCardsActionOff,
        AtkingDrawCards,
        EventAtkingDrawCardsActionOff,
        EventAtkingDrawCardsAddOnce,
        EventAtkingDrawCardsContinue,
        BattleDeckShuffle,
        AtkingDestroyCards,
        EventAtkingDestroyCardsActionOff,
        AtkingGiveCards,
        EventAtkingGiveCardsActionOff,
        AtkingGetUsedCards,
        EventAtkingGetUsedCardsActionOff,
        AtkingOneSelfCardControl,
        EventAtkingOneSelfCardControlActionOff,
        PersonMaxCardsNumControl,
        BattleInsertEventCard,
        PersonAddActualStatus,
        EventAddActualStatusData,
        ActualStatusEnd,
        PersonRemoveActualStatus,
        EventRemoveActualStatusActionOff,
        PersonAddBuff,
        BuffTurnEnd,
        PersonRemoveBuffAll,
        PersonRemoveBuffSelect,
        PersonBuffTurnChange,
        EventRemoveBuffActionOff
    }
}
