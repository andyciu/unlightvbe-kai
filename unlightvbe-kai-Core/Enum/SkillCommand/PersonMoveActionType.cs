namespace unlightvbe_kai_core.Enum.SkillCommand
{
    /// <summary>
    /// 執行指令-人物角色移動階段行動控制列舉
    /// </summary>
    public enum PersonMoveActionType
    {
        None,
        BarMoveLeft,
        BarMoveStay,
        BarMoveRight,
        BarMoveChange,
    }

    static class PersonMoveActionTypeMethods
    {
        public static MoveBarSelectType ToMoveBarSelectType(this PersonMoveActionType type)
        {
            return type switch
            {
                PersonMoveActionType.BarMoveLeft => MoveBarSelectType.Left,
                PersonMoveActionType.BarMoveStay => MoveBarSelectType.Stay,
                PersonMoveActionType.BarMoveRight => MoveBarSelectType.Right,
                PersonMoveActionType.BarMoveChange => MoveBarSelectType.Change,
                PersonMoveActionType.None => MoveBarSelectType.None,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
