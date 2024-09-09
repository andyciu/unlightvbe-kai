using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum
{
    public enum ActionCardOwner
    {
        System,
        Player1,
        Player2
    }

    static class ActionCardOwnerMethods
    {
        /// <summary>
        /// 比較是否相等(玩家絕對方)
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool Equal(this ActionCardOwner owner, UserPlayerType player)
        {
            return owner switch
            {
                ActionCardOwner.System => false,
                ActionCardOwner.Player1 => player == UserPlayerType.Player1,
                ActionCardOwner.Player2 => player == UserPlayerType.Player2,
                _ => false
            };
        }

        /// <summary>
        /// 轉換至相對狀態
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static ActionCardRelativeOwner ToRelative(this ActionCardOwner owner, UserPlayerType player)
        {
            return owner switch
            {
                ActionCardOwner.System => ActionCardRelativeOwner.System,
                ActionCardOwner.Player1 => player == UserPlayerType.Player1 ? ActionCardRelativeOwner.Self : ActionCardRelativeOwner.Opponent,
                ActionCardOwner.Player2 => player == UserPlayerType.Player2 ? ActionCardRelativeOwner.Self : ActionCardRelativeOwner.Opponent,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
