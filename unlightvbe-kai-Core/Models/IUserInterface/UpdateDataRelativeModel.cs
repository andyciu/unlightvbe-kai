using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.IUserInterface
{
    public class UpdateDataRelativeModel
    {
        public UpdateDataRelativeType Type { get; set; }
        public UserPlayerRelativeType Player { get; set; }
        public int Value { get; set; }
        public string? Message { get; set; }
    }
}
