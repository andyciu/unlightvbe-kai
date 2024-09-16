using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum
{
    public enum UpdateDataType
    {
        None = 0,
        TurnNumber,
        DeckCount,
        OpponentHoldingCardCount,
        OpponentPlayingCardCount,
        PlayerDistanceType,
        OKButtonOpen,
        OpponentCharacterChangeBegin,
        OpponentCharacterChangeAction,
        ActiveSkillLineLight
    }
}
