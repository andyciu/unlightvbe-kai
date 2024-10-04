using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.IUserInterface
{
    public class UpdateDataMultiModel
    {
        public UpdateDataMultiType Type { get; set; }
        public int Self { get; set; }
        public int Opponent { get; set; }
    }
}
