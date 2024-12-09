using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.StageMessage
{
    public record StageMessageModel_46 : IStageMessageModel<StageMessageModel_46>
    {
        public required CommandPlayerRelativeTwoVersionType TargetPlayer { get; init; }
        public required int TargetCharacterIndex { get; init; }
        public required CharacterHPDamageType DamageType { get; init; }
        public required int DamageValue { get; init; }
        public required CommandPlayerRelativeThreeVersionType TriggerPlayer { get; init; }
        public required TriggerSkillType TriggerSkill { get; init; }
    }
}
