using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 牌堆集合洗牌
        /// </summary>
        /// <param name="origdeck">原始集合</param>
        /// <returns></returns>
        private Dictionary<int, Card> ShuffleDeck(Dictionary<int, Card> origdeck)
        {
            var tmpdeck = new Dictionary<int, Card>(origdeck);
            var newdeck = new Dictionary<int, Card>();
            Random rnd = new(DateTime.Now.Millisecond);

            while (tmpdeck.Count > 0)
            {
                var tmpnum = rnd.Next(tmpdeck.Count);
                newdeck.Add(tmpdeck.ElementAt(tmpnum).Key, tmpdeck.ElementAt(tmpnum).Value);
                tmpdeck.Remove(tmpdeck.ElementAt(tmpnum).Key);
            }

            return newdeck;
        }

        /// <summary>
        /// 墓地牌洗牌前重新倒回牌堆
        /// </summary>
        private void GraveyardDeckReUse()
        {
            foreach (var card in CardDecks[(int)CardDeckType.Graveyard])
            {
                card.Value.Location = ActionCardLocation.Deck;
                card.Value.Owner = ActionCardOwner.System;

                DeckCardMove(card.Value, CardDeckType.Graveyard, CardDeckType.Deck);
            }
        }

        /// <summary>
        /// 卡牌於牌堆集合間移動
        /// </summary>
        /// <param name="card">卡牌</param>
        /// <param name="origType">目前所在集合</param>
        /// <param name="destType">目標集合</param>
        private void DeckCardMove(Card card, CardDeckType origType, CardDeckType destType)
        {
            CardDecks[(int)destType].Add(card.Number, card);
            CardDecks[(int)origType].Remove(card.Number);

            CardDeckIndex[card.Number] = destType;
        }

        /// <summary>
        /// 取得卡牌編號
        /// </summary>
        /// <param name="cardDeckType">目標集合</param>
        /// <returns></returns>
        private int GetCardIndex(CardDeckType cardDeckType)
        {
            int newNum = CardDeckIndex.Count + 1;
            CardDeckIndex.Add(newNum, cardDeckType);

            return newNum;
        }

        /// <summary>
        /// (初始階段)匯入場地行動卡資訊
        /// </summary>
        private void ImportActionCardToDeck()
        {
            var cardlist = SampleData.GetCardList_Deck();

            foreach (var card in cardlist)
            {
                int tmpnum = GetCardIndex(CardDeckType.Deck);
                card.Number = tmpnum;
                card.Owner = ActionCardOwner.System;
                card.Location = ActionCardLocation.Deck;

                CardDecks[(int)CardDeckType.Deck].Add(tmpnum, card);
            }
        }

        /// <summary>
        /// (初始階段)匯入玩家事件卡資訊
        /// </summary>
        private void ImportEventCardToDeck()
        {
            for (int i = 0; i < PlayerDatas.Length; i++)
            {
                var player = PlayerDatas[i];
                foreach (var sub in player.Player.Decks[player.DeckIndex].Deck_Subs)
                {
                    foreach (var card in sub.eventCards)
                    {
                        CardDeckType cardDeckType = GetCardDeckType((UserPlayerType)i, ActionCardLocation.Deck);

                        int tmpnum = GetCardIndex(cardDeckType);
                        card.Number = tmpnum;
                        card.Owner = ActionCardOwner.System;
                        card.Location = ActionCardLocation.Deck;

                        CardDecks[(int)cardDeckType].Add(tmpnum, card);
                    }
                }
            }
        }

        /// <summary>
        /// 玩家事件卡預設補充
        /// </summary>
        private void EvnetCardComplement()
        {
            Random rnd = new(DateTime.Now.Millisecond);

            for (int n = 0; n < PlayerDatas.Length; n++)
            {
                CardDeckType cardDeckType = GetCardDeckType((UserPlayerType)n, ActionCardLocation.Deck);
                if (CardDecks[(int)cardDeckType].Count < TurnMaxNum)
                {
                    for (int i = CardDecks[(int)cardDeckType].Count; i < TurnMaxNum; i++)
                    {
                        int tmpnum = GetCardIndex(cardDeckType);
                        Card tmpcard = GetDefaultEventCard(rnd.Next(3));
                        tmpcard.Number = tmpnum;
                        tmpcard.Owner = ActionCardOwner.System;
                        tmpcard.Location = ActionCardLocation.Deck;
                        CardDecks[(int)cardDeckType].Add(tmpnum, tmpcard);
                    }
                }
            }
        }

        private EventCard GetDefaultEventCard(int typenum)
        {
            return typenum switch
            {
                0 => new EventCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = 1,
                    LowerType = ActionCardType.ATK_Sword,
                    LowerNum = 1,
                },
                1 => new EventCard
                {
                    UpperType = ActionCardType.ATK_Gun,
                    UpperNum = 1,
                    LowerType = ActionCardType.ATK_Gun,
                    LowerNum = 1,
                },
                2 => new EventCard
                {
                    UpperType = ActionCardType.DEF,
                    UpperNum = 1,
                    LowerType = ActionCardType.DEF,
                    LowerNum = 1,
                },
                _ => new EventCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = 1,
                    LowerType = ActionCardType.ATK_Sword,
                    LowerNum = 1,
                },
            };
        }

        private CardDeckType GetCardDeckType(ActionCardOwner owner, ActionCardLocation location)
        {
            return location switch
            {
                ActionCardLocation.Hold => owner switch
                {
                    ActionCardOwner.Player1 => CardDeckType.Hold_P1,
                    ActionCardOwner.Player2 => CardDeckType.Hold_P2,
                    _ => throw new NotImplementedException(),
                },
                ActionCardLocation.Play => owner switch
                {
                    ActionCardOwner.Player1 => CardDeckType.Play_P1,
                    ActionCardOwner.Player2 => CardDeckType.Play_P2,
                    _ => throw new NotImplementedException(),
                },
                ActionCardLocation.Graveyard => CardDeckType.Graveyard,
                ActionCardLocation.Fold => CardDeckType.Fold,
                ActionCardLocation.Deck => owner switch
                {
                    ActionCardOwner.Player1 => CardDeckType.Event_P1,
                    ActionCardOwner.Player2 => CardDeckType.Event_P2,
                    ActionCardOwner.System => CardDeckType.Deck,
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };
        }

        private CardDeckType GetCardDeckType(UserPlayerType player, ActionCardLocation location)
        {
            return location switch
            {
                ActionCardLocation.Hold => player switch
                {
                    UserPlayerType.Player1 => CardDeckType.Hold_P1,
                    UserPlayerType.Player2 => CardDeckType.Hold_P2,
                    _ => throw new NotImplementedException(),
                },
                ActionCardLocation.Play => player switch
                {
                    UserPlayerType.Player1 => CardDeckType.Play_P1,
                    UserPlayerType.Player2 => CardDeckType.Play_P2,
                    _ => throw new NotImplementedException(),
                },
                ActionCardLocation.Deck => player switch
                {
                    UserPlayerType.Player1 => CardDeckType.Event_P1,
                    UserPlayerType.Player2 => CardDeckType.Event_P2,
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };
        }
    }
}
