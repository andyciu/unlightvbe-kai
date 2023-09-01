using System;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public class BattleSystem
    {
        protected PlayerData Player1 { get; set; }
        protected PlayerData Player2 { get; set; }
        protected bool IsFinish { get; set; }
        protected MultiUserInterfaceAdapter MultiUIAdapter { get; set; }
        protected List<Dictionary<int, Card>> CardDecks { get; set; }
        protected Dictionary<int, CardDeckType> CardDeckIndex { get; set; }
        protected int TurnNum { get; set; }

        public BattleSystem(Player player1, Player player2, int deckIndex_P1, int deckIndex_P2)
        {
            Player1 = new(player1, deckIndex_P1);
            Player2 = new(player2, deckIndex_P2);
        }

        public void SetUserInterface(IUserInterfaceAsync userInterface_P1, IUserInterfaceAsync userInterface_P2)
        {
            Player1.UserInterface = userInterface_P1;
            Player2.UserInterface = userInterface_P2;
            MultiUIAdapter = new(userInterface_P1, userInterface_P2);
        }

        public void Start()
        {
            InitialData();
            StartScreenPhase();
            DrawPhase();

            while (true)
            {
                var tmpaction = Player1.UserInterface.ReadAction();
                Player1.UserInterface.ShowBattleMessage(tmpaction.Type.ToString() + " " + tmpaction.Message);
                if (tmpaction.Type == ReadActionType.OKButtonClick)
                {
                    break;
                }
            }
        }

        public bool CheckisFinish()
        {
            return IsFinish;
        }

        private void InitialData()
        {
            //CardDeck
            CardDecks = new List<Dictionary<int, Card>>();
            CardDeckIndex = new Dictionary<int, CardDeckType>();

            foreach (var type in (CardDeckType[])System.Enum.GetValues(typeof(CardDeckType)))
            {
                CardDecks.Add(new Dictionary<int, Card>());
            }

            ImportActionCardToDeck();
            ImportEventCardToDeck();

            CardDecks[(int)CardDeckType.Deck] = ShuffleDeck(CardDecks[(int)CardDeckType.Deck]);
            CardDecks[(int)CardDeckType.Event_P1] = ShuffleDeck(CardDecks[(int)CardDeckType.Event_P1]);
            CardDecks[(int)CardDeckType.Event_P2] = ShuffleDeck(CardDecks[(int)CardDeckType.Event_P2]);

            Player1.HoldMaxCount = 5;
            Player2.HoldMaxCount = 5;

        }

        /// <summary>
        /// 開始畫面階段
        /// </summary>
        private void StartScreenPhase()
        {
            MultiUIAdapter.ShowStartScreen(new()
            {
                Player1_Id = Player1.Player.PlayerId,
                Player2_Id = Player2.Player.PlayerId,
                Player1_DeckIndex = Player1.DeckIndex,
                Player2_DeckIndex = Player2.DeckIndex,
            });
        }

        /// <summary>
        /// 抽牌階段
        /// </summary>
        private void DrawPhase()
        {
            int waitToDeal_P1 = Player1.HoldMaxCount - CardDecks[(int)CardDeckType.Hold_P1].Count;
            int waitToDeal_P2 = Player2.HoldMaxCount - CardDecks[(int)CardDeckType.Hold_P2].Count;
            List<Card> dealCards_p1 = new List<Card>();
            List<Card> dealCards_p2 = new List<Card>();

            //回合數增加
            TurnNum += 1;
            MultiUIAdapter.UpdateData(new()
            {
                Type = UpdateDataType.TurnNumber,
                Value = TurnNum
            });

            //若牌堆集合卡牌數量不足時進行洗牌
            if (CardDecks[(int)CardDeckType.Deck].Count < waitToDeal_P1 + waitToDeal_P2)
            {
                GraveyardDeckReUse();
                CardDecks[(int)CardDeckType.Deck] = ShuffleDeck(CardDecks[(int)CardDeckType.Deck]);
            }

            //同步牌堆數量
            MultiUIAdapter.UpdateData(new()
            {
                Type = UpdateDataType.DeckNumber,
                Value = CardDecks[(int)CardDeckType.Deck].Count
            });

            var dealside = ActionCardOwner.Player1; //目前發牌方
            //發牌(行動卡)
            while (waitToDeal_P1 > 0 || waitToDeal_P2 > 0)
            {
                var tmpcard = CardDecks[(int)CardDeckType.Deck].ElementAt(0).Value;
                if (dealside == ActionCardOwner.Player1)
                {
                    dealside = ActionCardOwner.Player2;
                    if (waitToDeal_P1 > 0)
                    {
                        tmpcard.Location = ActionCardLocation.Hold;
                        tmpcard.Owner = ActionCardOwner.Player1;

                        dealCards_p1.Add(new ActionCard(tmpcard));
                        DeckCardMove(tmpcard, CardDeckType.Deck, CardDeckType.Hold_P1);
                        waitToDeal_P1--;
                        continue;
                    }
                }
                if (dealside == ActionCardOwner.Player2)
                {
                    dealside = ActionCardOwner.Player1;
                    if (waitToDeal_P2 > 0)
                    {
                        tmpcard.Location = ActionCardLocation.Hold;
                        tmpcard.Owner = ActionCardOwner.Player2;

                        dealCards_p2.Add(new ActionCard(tmpcard));
                        DeckCardMove(tmpcard, CardDeckType.Deck, CardDeckType.Hold_P2);
                        waitToDeal_P2--;
                        continue;
                    }
                }
            }

            MultiUIAdapter.DrawActionCard(dealCards_p1, dealCards_p2);

            //發牌(事件卡)

        }

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
            CardDeckType cardDeckType;
            PlayerData[] playerDataList = new PlayerData[2] { Player1, Player2 };

            foreach (var player in playerDataList)
            {
                if (player.Equals(Player1))
                {
                    cardDeckType = CardDeckType.Event_P1;
                }
                else
                {
                    cardDeckType = CardDeckType.Event_P2;
                }

                foreach (var sub in player.Player.Decks[player.DeckIndex].Deck_Subs)
                {
                    foreach (var card in sub.eventCards)
                    {
                        int tmpnum = GetCardIndex(cardDeckType);
                        card.Number = tmpnum;
                        card.Owner = ActionCardOwner.System;
                        card.Location = ActionCardLocation.Deck;

                        CardDecks[(int)cardDeckType].Add(tmpnum, card);
                    }
                }
            }
        }
    }
}