namespace unlightvbe_kai_core.Enum.SkillCommand
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
        /// <summary>
        /// 系統回合數控制
        /// </summary>
        BattleTurnControl,
        /// <summary>
        /// 發布訊息效果執行
        /// </summary>
        BattleSendMessage,
        /// <summary>
        /// 擲骰效果執行
        /// </summary>
        BattleStartDice,
        EventPlayerAllActionOff,
        /// <summary>
        /// 角色相對距離控制
        /// </summary>
        BattleMoveControl,
        /// <summary>
        /// 原應執行之距離變更無效化
        /// </summary>
        EventMoveActionOff,
        /// <summary>
        /// 人物移動階段總移動量控制
        /// </summary>
        PersonMoveControl,
        /// <summary>
        /// 人物角色移動階段行動控制
        /// </summary>
        PersonMoveActionChange,
        /// <summary>
        /// 人物角色優先攻擊控制
        /// </summary>
        PersonAttackFirstControl,
        /// <summary>
        /// 攻擊/防禦階段系統骰數變化量控制
        /// </summary>
        EventTotalDiceChange,
        /// <summary>
        /// 攻擊/防禦階段角色白值能力對骰數變化量控制
        /// </summary>
        EventPersonAbilityDiceChange,
        /// <summary>
        /// 攻擊/防禦階段擲骰前系統總骰數直接控制
        /// </summary>
        PersonTotalDiceControl,
        /// <summary>
        /// 擲骰後正面骰數控制
        /// </summary>
        AttackTrueDiceControl,
        /// <summary>
        /// 人物角色血量控制
        /// </summary>
        PersonBloodControl,
        /// <summary>
        /// 原應執行之傷害無效化
        /// </summary>
        EventBloodActionOff,
        /// <summary>
        /// 原應執行之傷害效果變更
        /// </summary>
        EventBloodActionChange,
        /// <summary>
        /// 傷害反射效果執行
        /// </summary>
        EventBloodReflection,
        /// <summary>
        /// 原應執行之回復無效化
        /// </summary>
        EventHealActionOff,
        /// <summary>
        /// 原應執行之回復效果變更
        /// </summary>
        EventHealActionChange,
        /// <summary>
        /// 回復反射效果執行
        /// </summary>
        EventHealReflection,
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
        /// <summary>
        /// 人物角色新增異常狀態
        /// </summary>
        PersonAddBuff,
        /// <summary>
        /// 異常狀態宣告當回合結束
        /// </summary>
        BuffTurnEnd,
        /// <summary>
        /// 異常狀態宣告結束
        /// </summary>
        BuffEnd,
        /// <summary>
        /// 人物角色移除異常狀態(全部)
        /// </summary>
        PersonRemoveBuffAll,
        /// <summary>
        /// 人物角色移除異常狀態(指定)
        /// </summary>
        PersonRemoveBuffSelect,
        /// <summary>
        /// 人物角色異常狀態變更回合數
        /// </summary>
        PersonBuffTurnChange,
        /// <summary>
        /// 原應執行之異常狀態消除無效化
        /// </summary>
        EventRemoveBuffActionOff
    }
}
