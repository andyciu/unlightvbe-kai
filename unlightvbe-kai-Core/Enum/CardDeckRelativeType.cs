using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum
{
    /// <summary>
    /// 卡牌集合列舉(相對)
    /// </summary>
    public enum CardDeckRelativeType
    {
        Deck,
        Graveyard,
        Event_Self,
        Event_Oppenent,
        Hold_Self,
        Hold_Oppenent,
        Play_Self,
        Play_Oppenent,
        Fold
    }
}
