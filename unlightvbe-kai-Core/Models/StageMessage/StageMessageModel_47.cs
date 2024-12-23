using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.StageMessage
{
    public record StageMessageModel_47 : IStageMessageModel<StageMessageModel_47>
    {
        public required CommandPlayerDistanceType DistanceBefore { get; init; }
        public required CommandPlayerDistanceType DistanceAfter { get; init; }
        public required CommandPlayerRelativeThreeVersionType TriggerPlayer { get; init; }
    }
}
