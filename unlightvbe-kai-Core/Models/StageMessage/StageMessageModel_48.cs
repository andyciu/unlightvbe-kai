using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.StageMessage
{
    public record StageMessageModel_48 : IStageMessageModel<StageMessageModel_48>
    {
        public CommandPlayerRelativeTwoVersionType TargetPlayer { get; init; }
        public int TargetCharacterIndex { get; init; }
        public int HealValue { get; init; }
        public CommandPlayerRelativeThreeVersionType TriggerPlayer { get; init; }
        public TriggerSkillType TriggerSkill { get; init; }
    }
}
