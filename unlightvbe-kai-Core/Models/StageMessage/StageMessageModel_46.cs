using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.StageMessage
{
    public record StageMessageModel_46 : IStageMessageModel<StageMessageModel_46>
    {
        public CommandPlayerRelativeTwoVersionType TargetPlayer { get; init; }
        public int TargetCharacterIndex { get; init; }
        public CharacterHPDamageType DamageType { get; init; }
        public int DamageValue { get; init; }
        public CommandPlayerRelativeThreeVersionType TriggerPlayer { get; init; }
        public TriggerSkillType TriggerSkill { get; init; }
    }
}
