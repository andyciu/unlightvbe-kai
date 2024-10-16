namespace unlightvbe_kai_core.Enum.SkillCommand
{
    /// <summary>
    /// 執行指令參數玩家方列舉(相對-2種類)
    /// </summary>
    public enum CommandPlayerRelativeTwoVersionType
    {
        Self = 1,
        Opponent
    }

    static class PlayerRelativeTypeMethods
    {
        public static UserPlayerRelativeType ToUserPlayerRelativeType(this CommandPlayerRelativeTwoVersionType type)
        {
            return type switch
            {
                CommandPlayerRelativeTwoVersionType.Self => UserPlayerRelativeType.Self,
                CommandPlayerRelativeTwoVersionType.Opponent => UserPlayerRelativeType.Opponent,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
