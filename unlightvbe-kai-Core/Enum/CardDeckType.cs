using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Enum
{
    public enum CardDeckType
    {
        Deck,
        Graveyard,
        Event_P1,
        Event_P2,
        Hold_P1,
        Hold_P2,
        Play_P1,
        Play_P2,
        Fold
    }

    static class CardDeckTypeMethods
    {
        /// <summary>
        /// 轉換至相對狀態
        /// </summary>
        /// <param name="type"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static CardDeckRelativeType ToRelative(this CardDeckType type, UserPlayerType player)
        {
            return type switch
            {
                CardDeckType.Event_P1 => player == UserPlayerType.Player1 ? CardDeckRelativeType.Event_Self : CardDeckRelativeType.Event_Oppenent,
                CardDeckType.Event_P2 => player == UserPlayerType.Player2 ? CardDeckRelativeType.Event_Self : CardDeckRelativeType.Event_Oppenent,
                CardDeckType.Hold_P1 => player == UserPlayerType.Player1 ? CardDeckRelativeType.Hold_Self : CardDeckRelativeType.Hold_Oppenent,
                CardDeckType.Hold_P2 => player == UserPlayerType.Player2 ? CardDeckRelativeType.Hold_Self : CardDeckRelativeType.Hold_Oppenent,
                CardDeckType.Play_P1 => player == UserPlayerType.Player1 ? CardDeckRelativeType.Play_Self : CardDeckRelativeType.Play_Oppenent,
                CardDeckType.Play_P2 => player == UserPlayerType.Player2 ? CardDeckRelativeType.Play_Self : CardDeckRelativeType.Play_Oppenent,
                CardDeckType.Deck => CardDeckRelativeType.Deck,
                CardDeckType.Graveyard => CardDeckRelativeType.Graveyard,
                CardDeckType.Fold => CardDeckRelativeType.Fold,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
