using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Models.IUserInterface
{
    public class DrawActionCardModel
    {
        public List<CardModel> SelfCards { get; set; }
        public int OpponentCardCount { get; set; }
    }
}
