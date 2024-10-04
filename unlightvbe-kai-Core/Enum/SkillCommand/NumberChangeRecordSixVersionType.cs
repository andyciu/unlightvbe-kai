namespace unlightvbe_kai_core.Enum.SkillCommand
{
    /// <summary>
    /// 執行指令-變化量控制紀錄列舉(6種類)
    /// </summary>
    public enum NumberChangeRecordSixVersionType
    {
        /// <summary>
        /// 加
        /// </summary>
        Addition = 1,
        /// <summary>
        /// 減
        /// </summary>
        Subtraction,
        /// <summary>
        /// 乘
        /// </summary>
        Multiplication,
        /// <summary>
        /// 除(尾數捨去)
        /// </summary>
        Division_Floor,
        /// <summary>
        /// 除(尾數進位)
        /// </summary>
        Division_Ceiling,
        /// <summary>
        /// 指定
        /// </summary>
        Assign
    }
}
