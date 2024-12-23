namespace unlightvbe_kai_core.Models.UserInterface
{
    public record ChangeCharacterActionModel
    {
        public required bool IsChange { get; init; }
        public required string NewCharacterVBEID { get; init; }
    }
}
