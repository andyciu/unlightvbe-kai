using unlightvbe_kai_core.Enum.UserInterface;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record UpdateDataMultiModel
    {
        public required UpdateDataMultiType Type { get; init; }
        public int Self { get; init; }
        public int Opponent { get; init; }
    }
}
