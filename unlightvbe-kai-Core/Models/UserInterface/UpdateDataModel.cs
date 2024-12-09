using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record UpdateDataModel
    {
        public required UpdateDataType Type { get; init; }
        public int Value { get; init; }
        public string? Message { get; init; }
    }
}
