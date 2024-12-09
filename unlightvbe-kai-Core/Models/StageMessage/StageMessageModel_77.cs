using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.StageMessage
{
    public record StageMessageModel_77 : IStageMessageModel<StageMessageModel_77>
    {
        public required CommandPlayerRelativeTwoVersionType TargetPlayer { get; init; }
        public required SkillType SkillType { get; init; }
        public required string Identifier { get; init; }
    }
}
