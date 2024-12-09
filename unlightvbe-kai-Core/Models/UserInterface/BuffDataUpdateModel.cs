using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.UserInterface
{
    public record BuffDataUpdateModel
    {
        public required Dictionary<UserPlayerRelativeType, Dictionary<string, List<BuffDataBaseModel>>> Datas { get; init; }
    }
}
