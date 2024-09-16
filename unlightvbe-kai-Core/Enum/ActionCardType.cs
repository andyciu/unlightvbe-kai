using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum
{
    /// <summary>
    /// 行動卡類別
    /// </summary>
    public enum ActionCardType
    {
        /// <summary>
        /// 無(技能條件判斷用)
        /// </summary>
        None,
        /// <summary>
        /// 劍
        /// </summary>
        ATK_Sword,
        /// <summary>
        /// 防
        /// </summary>
        DEF,
        /// <summary>
        /// 移
        /// </summary>
        MOV,
        /// <summary>
        /// 特
        /// </summary>
        SPE,
        /// <summary>
        /// 槍
        /// </summary>
        ATK_Gun,
        /// <summary>
        /// 機會
        /// </summary>
        DRAW,
        /// <summary>
        /// 詛咒
        /// </summary>
        BRK,
        /// <summary>
        /// HP恢復
        /// </summary>
        HPL,
        /// <summary>
        /// 聖水
        /// </summary>
        HPW
    }
}
