using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.StageMessage
{
    public record StageMessageModel_48 : IStageMessageModel<StageMessageModel_48>
    {
        public required CommandPlayerRelativeTwoVersionType TargetPlayer { get; init; }
        public required int TargetCharacterIndex { get; init; }
        public required int HealValue { get; init; }
        public required CommandPlayerRelativeThreeVersionType TriggerPlayer { get; init; }
        public required TriggerSkillType TriggerSkill { get; init; }
    }
}
