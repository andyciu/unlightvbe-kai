using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum.SkillCommand
{
    /// <summary>
    /// 觸發事件方(絕對)
    /// </summary>
    public enum TriggerPlayerType
    {
        System,
        Player1,
        Player2
    }

    static class TriggerPlayerTypeMethods
    {
        /// <summary>
        /// 比較是否相等(玩家絕對方)
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool Equal(this TriggerPlayerType owner, UserPlayerType player)
        {
            return owner switch
            {
                TriggerPlayerType.System => false,
                TriggerPlayerType.Player1 => player == UserPlayerType.Player1,
                TriggerPlayerType.Player2 => player == UserPlayerType.Player2,
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
        public static TriggerPlayerRelativeType ToRelative(this TriggerPlayerType owner, UserPlayerType player)
        {
            return owner switch
            {
                TriggerPlayerType.System => TriggerPlayerRelativeType.System,
                TriggerPlayerType.Player1 => player == UserPlayerType.Player1 ? TriggerPlayerRelativeType.Self : TriggerPlayerRelativeType.Opponent,
                TriggerPlayerType.Player2 => player == UserPlayerType.Player2 ? TriggerPlayerRelativeType.Self : TriggerPlayerRelativeType.Opponent,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// 轉換至UserPlayerType
        /// </summary>
        /// <param name="owner"></param>
        /// <returns></returns>
        public static UserPlayerType? ToUserPlayerType(this TriggerPlayerType owner)
        {
            return owner switch {
                TriggerPlayerType.Player1 => UserPlayerType.Player1,
                TriggerPlayerType.Player2 => UserPlayerType.Player2,
                _ => null
            };
        }
    }
}
