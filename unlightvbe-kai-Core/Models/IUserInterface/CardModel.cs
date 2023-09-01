using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
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
        public ActionCardOwner Owner { get; set; }
        public ActionCardLocation Location { get; set; }
    }
}
