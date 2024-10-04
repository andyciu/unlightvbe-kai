namespace unlightvbe_kai_core.Enum
{
    /// <summary>
    /// 遊戲回合階段
    /// </summary>
    public enum PhaseType
    {
        None = 0,
        /// <summary>
        /// 發牌階段
        /// </summary>
        Draw,
        /// <summary>
        /// 移動階段
        /// </summary>
        Move,
        /// <summary>
        /// 攻擊階段
        /// </summary>
        Attack,
        /// <summary>
        /// 防禦階段
        /// </summary>
        Defense,
        /// <summary>
        /// 回合結束階段
        /// </summary>
        TurnEnd
    }
}
