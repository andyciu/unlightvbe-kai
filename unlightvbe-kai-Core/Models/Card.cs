using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models
{
    public abstract class Card
    {
        public int Number { get; set; }
        public ActionCardType UpperType { get; set; }
        public int UpperNum { get; set; }
        public ActionCardType LowerType { get; set; }
        public int LowerNum { get; set; }
        public ActionCardOwner Owner { get; set; }
        public ActionCardLocation Location { get; set; }

        public Card() { }

        public Card(Card card)
        {
            Number = card.Number;
            UpperType = card.UpperType;
            UpperNum = card.UpperNum;
            LowerType = card.LowerType;
            LowerNum = card.LowerNum;
            Owner = card.Owner;
            Location = card.Location;
        }

        public void Reverse()
        {
            var tmpType = UpperType;
            var tmpNum = UpperNum;

            UpperType = LowerType;
            UpperNum = LowerNum;

            LowerType = tmpType;
            LowerNum = tmpNum;
        }
    }
}
