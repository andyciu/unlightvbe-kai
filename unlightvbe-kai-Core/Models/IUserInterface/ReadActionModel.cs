using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.IUserInterface
{
    public class ReadActionModel
    {
        public UserActionType Type { get; set; }
        public int Value { get; set; }
        public string? Message { get; set; }
    }
}
