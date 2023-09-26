using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum
{
    public enum ReadActionReceiveType
    {
        None = 0,
        HoldingCard,
        PlayingCard,
        HoldingCardReverse,
        PlayingCardReverse,
        BarMoveLeft,
        BarMoveRight,
        BarMoveStay,
        BarMoveChange,
        OKButtonClick,
        ChangeCharacter
    }
}
