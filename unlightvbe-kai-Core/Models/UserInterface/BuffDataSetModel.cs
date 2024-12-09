using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record BuffDataSetModel()
    {
        public required UserPlayerRelativeType Player { get; init; }
        public required string CharacterVBEID { get; init; }
        public required BuffDataBaseModel BuffData { get; init; }
    }
}
