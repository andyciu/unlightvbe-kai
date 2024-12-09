namespace unlightvbe_kai_core.Models.UserInterface
{
    public record DrawCardModel
    {
        public required List<CardModel> SelfCards { get; init; }
        public required int OpponentCardCount { get; init; }
    }
}
