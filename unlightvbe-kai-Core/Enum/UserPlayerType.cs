using unlightvbe_kai_core.Enum.SkillCommand;

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

        /// <summary>
        /// 轉換至相對狀態
        /// </summary>
        /// <param name="player"></param>
        /// <param name="currentPlayer">目前玩家方</param>
        /// <returns></returns>
        public static UserPlayerRelativeType ToRelative(this UserPlayerType player, UserPlayerType currentPlayer)
        {
            return player == currentPlayer ? UserPlayerRelativeType.Self : UserPlayerRelativeType.Opponent;
        }

        public static TriggerPlayerType ToTriggerPlayerType(this UserPlayerType player)
        {
            return player switch
            {
                UserPlayerType.Player1 => TriggerPlayerType.Player1,
                UserPlayerType.Player2 => TriggerPlayerType.Player2,
                _ => throw new NotImplementedException()
            };
        }
    }
}
