namespace unlightvbe_kai_core.Models.UserInterface
{
    public record ShowStartScreenModel
    {
        public required int PlayerSelfId { get; init; }
        public required int PlayerOpponentId { get; init; }
        public required List<string> PlayerSelf_CharacterVBEID { get; init; }
        public required List<string> PlayerOpponent_CharacterVBEID { get; init; }
    }
}
