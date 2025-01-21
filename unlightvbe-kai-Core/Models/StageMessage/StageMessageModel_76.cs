using unlightvbe_kai_core.Enum.Skill;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.StageMessage
{
    public record StageMessageModel_76 : IStageMessageModel<StageMessageModel_76>
    {
        public required CommandPlayerRelativeTwoVersionType TargetPlayer { get; init; }
        public required SkillType SkillType { get; init; }
        public required string Identifier { get; init; }
    }
}
