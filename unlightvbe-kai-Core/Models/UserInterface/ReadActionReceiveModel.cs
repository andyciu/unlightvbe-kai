using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record ReadActionReceiveModel
    {
        public required ReadActionReceiveType Type { get; init; }
        public required bool IsSuccess { get; init; }
    }
}
