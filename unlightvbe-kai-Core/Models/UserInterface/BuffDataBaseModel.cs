namespace unlightvbe_kai_core.Models.UserInterface
{
    public record BuffDataBaseModel
    {
        public required string Identifier { get; init; }
        public required int Value { get; init; }
        public required int Total { get; init; }
    }
}
