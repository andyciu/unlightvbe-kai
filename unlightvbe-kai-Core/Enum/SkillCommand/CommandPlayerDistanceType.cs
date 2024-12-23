namespace unlightvbe_kai_core.Enum.SkillCommand
{
    public enum CommandPlayerDistanceType
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

    static class CommandPlayerDistanceTypeMethods
    {
        public static PlayerDistanceType ToPlayerDistanceType(this CommandPlayerDistanceType type)
        {
            return type switch
            {
                CommandPlayerDistanceType.Close => PlayerDistanceType.Close,
                CommandPlayerDistanceType.Middle => PlayerDistanceType.Middle,
                CommandPlayerDistanceType.Long => PlayerDistanceType.Long,
                _ => throw new NotImplementedException()
            };
        }
    }
}
