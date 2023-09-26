using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum
{
    /// <summary>
    /// 玩家雙方(絕對)
    /// </summary>
    public enum UserPlayerType
    {
        Player1,
        Player2
    }

    static class UserPlayerTypeMethods
    {
        /// <summary>
        /// 取得玩家相對對方型態
        /// </summary>
        /// <param name="player">我方玩家型態</param>
        /// <returns>對方玩家型態</returns>
        public static UserPlayerType GetOppenentPlayer(this UserPlayerType player)
        {
            return player == UserPlayerType.Player1 ? UserPlayerType.Player2 : UserPlayerType.Player1;
        }
    }
}
