using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum.SkillCommand
{
    /// <summary>
    /// 執行指令參數玩家方列舉(絕對)
    /// </summary>
    public enum CommandPlayerType
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
        public static bool Equal(this CommandPlayerType owner, UserPlayerType player)
        {
            return owner switch
            {
                CommandPlayerType.System => false,
                CommandPlayerType.Player1 => player == UserPlayerType.Player1,
                CommandPlayerType.Player2 => player == UserPlayerType.Player2,
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
        public static CommandPlayerRelativeThreeVersionType ToRelative(this CommandPlayerType owner, UserPlayerType player)
        {
            return owner switch
            {
                CommandPlayerType.System => CommandPlayerRelativeThreeVersionType.System,
                CommandPlayerType.Player1 => player == UserPlayerType.Player1 ? CommandPlayerRelativeThreeVersionType.Self : CommandPlayerRelativeThreeVersionType.Opponent,
                CommandPlayerType.Player2 => player == UserPlayerType.Player2 ? CommandPlayerRelativeThreeVersionType.Self : CommandPlayerRelativeThreeVersionType.Opponent,
                _ => throw new NotImplementedException(),
            };
        }

        public static UserPlayerType? ToUserPlayerType(this CommandPlayerType owner)
        {
            return owner switch {
                CommandPlayerType.Player1 => UserPlayerType.Player1,
                CommandPlayerType.Player2 => UserPlayerType.Player2,
                _ => null
            };
        }
    }
}
