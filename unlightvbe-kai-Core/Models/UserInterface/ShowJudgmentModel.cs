using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record ShowJudgmentModel
    {
        public required ShowJudgmentType Type { get; init; }
    }
}
