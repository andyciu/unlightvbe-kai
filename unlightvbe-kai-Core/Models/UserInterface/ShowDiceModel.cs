using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record ShowDiceModel
    {
        public required int[] DiceTotal { get; init; }
        public required int[] DiceTrue { get; init; }
        public required DiceType[] DiceType { get; init; }
    }
}
