using System.Text.Json.Serialization;
using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models.IUserInterface
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public class CardModel
    {
        public int Number { get; set; }
        public ActionCardType UpperType { get; set; }
        public int UpperNum { get; set; }
        public ActionCardType LowerType { get; set; }
        public int LowerNum { get; set; }
        public ActionCardRelativeOwner Owner { get; set; }
        public ActionCardLocation Location { get; set; }
    }
}
