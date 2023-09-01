using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Models
{
    public class ActionCard : Card
    {
        public ActionCard() { }
        public ActionCard(ActionCard card) : base(card) { }
        public ActionCard(Card card) : base(card) { }
    }
}
