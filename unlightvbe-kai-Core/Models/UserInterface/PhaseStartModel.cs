using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record PhaseStartModel
    {
        public required PhaseType Type { get; init; }
    }
}
