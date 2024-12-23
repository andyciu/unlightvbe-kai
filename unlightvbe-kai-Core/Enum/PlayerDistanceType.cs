using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Enum
{
    /// <summary>
    /// 玩家角色間相對距離列舉(系統)
    /// </summary>
    public enum PlayerDistanceType
    {
        /// <summary>
        /// 近距離
        /// </summary>
        Close = 1,
        /// <summary>
        /// 中距離
        /// </summary>
        Middle,
        /// <summary>
        /// 遠距離
        /// </summary>
        Long
    }

    static class PlayerDistanceTypeMethods
    {
        public static CommandPlayerDistanceType ToCommandPlayerDistanceType(this PlayerDistanceType type)
        {
            return type switch
            {
                PlayerDistanceType.Close => CommandPlayerDistanceType.Close,
                PlayerDistanceType.Middle => CommandPlayerDistanceType.Middle,
                PlayerDistanceType.Long => CommandPlayerDistanceType.Long,
                _ => throw new NotImplementedException()
            };
        }
    }
}
