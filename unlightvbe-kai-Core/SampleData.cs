using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public class SampleData
    {
        public static List<ActionCard> GetCardList_Deck()
        {
            var cards = new List<ActionCard>();

            for (int i = 1, j = 1; j <= 6; j++)
            {
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = i,
                    LowerType = ActionCardType.ATK_Gun,
                    LowerNum = j,
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = j,
                    LowerType = ActionCardType.ATK_Gun,
                    LowerNum = i,
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = j,
                    LowerType = ActionCardType.ATK_Gun,
                    LowerNum = j,
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.DEF,
                    UpperNum = j,
                    LowerType = ActionCardType.DEF,
                    LowerNum = j,
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.MOV,
                    UpperNum = i,
                    LowerType = ActionCardType.SPE,
                    LowerNum = j,
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.MOV,
                    UpperNum = j,
                    LowerType = ActionCardType.SPE,
                    LowerNum = i,
                });
            }

            return cards;
        }

        public static List<EventCard> GetCardList_Event()
        {
            var cards = new List<EventCard>();

            for (int j = 1; j <= 6; j++)
            {
                cards.Add(new EventCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = 6,
                    LowerType = ActionCardType.ATK_Gun,
                    LowerNum = 6,
                });
            }

            return cards;
        }
    }
}
