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
        public string Identifier { get; set; } = string.Empty;
        public bool IsReverse { get; private set; } = false;

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
            Identifier = card.Identifier;
            IsReverse = card.IsReverse;
        }

        public void Reverse()
        {
            var tmpType = UpperType;
            var tmpNum = UpperNum;

            UpperType = LowerType;
            UpperNum = LowerNum;

            LowerType = tmpType;
            LowerNum = tmpNum;

            IsReverse = !IsReverse;
        }
    }
}
