using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record ReadActionModel
    {
        public required UserActionType Type { get; init; }
        public int Value { get; init; }
        public string? Message { get; init; }
    }
}
