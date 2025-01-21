using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.UserInterface;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record CardModel
    {
        public required int Number { get; init; }
        public required ActionCardType UpperType { get; init; }
        public required int UpperNum { get; init; }
        public required ActionCardType LowerType { get; init; }
        public required int LowerNum { get; init; }
        public required ActionCardRelativeOwner Owner { get; init; }
        public required ActionCardLocation Location { get; init; }
        public required string Identifier { get; init; }
        public required bool IsReverse { get; init; }
    }
}
