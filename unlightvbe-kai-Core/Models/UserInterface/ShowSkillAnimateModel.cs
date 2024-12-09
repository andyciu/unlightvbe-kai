using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record ShowSkillAnimateModel
    {
        public required UserPlayerRelativeType Player { get; init; }
        public required string SkillID { get; init; }
    }
}
