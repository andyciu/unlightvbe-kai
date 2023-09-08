using System;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 玩家資料集
        /// </summary>
        protected PlayerData[] PlayerDatas { get; set; } = new PlayerData[2];
        /// <summary>
        /// 是否已結束對戰
        /// </summary>
        protected bool IsFinish { get; set; }
        /// <summary>
        /// 對雙方玩家使用者介面資訊配置器
        /// </summary>
        protected MultiUserInterfaceAdapter MultiUIAdapter { get; set; }
        /// <summary>
        /// 卡牌集合
        /// </summary>
        protected List<Dictionary<int, Card>> CardDecks { get; set; }
        /// <summary>
        /// 卡牌集合索引
        /// </summary>
        protected Dictionary<int, CardDeckType> CardDeckIndex { get; set; }
        /// <summary>
        /// 目前回合數
        /// </summary>
        private int TurnNum;
        /// <summary>
        /// 對戰最大回合數
        /// </summary>
        public int TurnMaxNum { get; set; } = 18;
        /// <summary>
        /// 對戰模式
        /// </summary>
        private PlayerVersusModeType PlayerVersusMode;


        public BattleSystem(Player player1, Player player2, int deckIndex_P1, int deckIndex_P2)
        {
            try
            {
                PlayerDatas[(int)UserPlayerType.Player1] = new(player1, deckIndex_P1);
                PlayerDatas[(int)UserPlayerType.Player2] = new(player2, deckIndex_P2);
                if (player1.Decks[deckIndex_P1].Deck_Subs.Count == 1 && player2.Decks[deckIndex_P2].Deck_Subs.Count == 1)
                {
                    PlayerVersusMode = PlayerVersusModeType.OneOnOne;
                }
                else if (player1.Decks[deckIndex_P1].Deck_Subs.Count == 3 && player2.Decks[deckIndex_P2].Deck_Subs.Count == 3)
                {
                    PlayerVersusMode = PlayerVersusModeType.ThreeOnThree;
                }
                else
                {
                    throw new ArgumentException("PlayerVersusMode No Match.");
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void SetUserInterface(IUserInterfaceAsync userInterface_P1, IUserInterfaceAsync userInterface_P2)
        {
            PlayerDatas[(int)UserPlayerType.Player1].UserInterface = userInterface_P1;
            PlayerDatas[(int)UserPlayerType.Player2].UserInterface = userInterface_P2;
            MultiUIAdapter = new(userInterface_P1, userInterface_P2, new UserActionProxy(this));
        }

        public void Start()
        {
            InitialData();
            StartScreenPhase();
            DrawPhase();
            MovePhase();
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

            EvnetCardComplement();

            foreach (var player in PlayerDatas)
            {
                player.HoldMaxCount = 5;
            }

        }

        /// <summary>
        /// 開始畫面階段
        /// </summary>
        private void StartScreenPhase()
        {
            MultiUIAdapter.ShowStartScreen(new()
            {
                Player1_Id = PlayerDatas[(int)UserPlayerType.Player1].Player.PlayerId,
                Player2_Id = PlayerDatas[(int)UserPlayerType.Player2].Player.PlayerId,
                Player1_DeckIndex = PlayerDatas[(int)UserPlayerType.Player1].DeckIndex,
                Player2_DeckIndex = PlayerDatas[(int)UserPlayerType.Player2].DeckIndex,
            });
        }

        /// <summary>
        /// 抽牌階段
        /// </summary>
        private void DrawPhase()
        {
            int waitToDeal_P1 = PlayerDatas[(int)UserPlayerType.Player1].HoldMaxCount - CardDecks[(int)CardDeckType.Hold_P1].Count;
            int waitToDeal_P2 = PlayerDatas[(int)UserPlayerType.Player2].HoldMaxCount - CardDecks[(int)CardDeckType.Hold_P2].Count;
            List<Card> dealCards_p1 = new List<Card>();
            List<Card> dealCards_p2 = new List<Card>();
            ActionCardOwner dealside = ActionCardOwner.Player1; //目前發牌方
            EventCard[] eventCards = new EventCard[2];

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
                Type = UpdateDataType.DeckCount,
                Value = CardDecks[(int)CardDeckType.Deck].Count
            });

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
            for (int n = 0; n < 2; n++)
            {
                if (n == 0) dealside = ActionCardOwner.Player1;
                else dealside = ActionCardOwner.Player2;

                var tmpcard = CardDecks[(int)GetCardDeckType(dealside, ActionCardLocation.Deck)].ElementAt(0).Value;
                tmpcard.Location = ActionCardLocation.Hold;
                tmpcard.Owner = dealside;

                eventCards[n] = new EventCard(tmpcard);
                DeckCardMove(tmpcard, GetCardDeckType(dealside, ActionCardLocation.Deck), GetCardDeckType(dealside, ActionCardLocation.Hold));
            }

            MultiUIAdapter.DrawEventCard(eventCards[0], eventCards[1]);
        }

        /// <summary>
        /// 移動階段
        /// </summary>
        private void MovePhase()
        {
            foreach (var player in PlayerDatas)
            {
                player.MoveBarSelect = MoveBarSelectType.None;
                player.IsOKButtonSelect = false;
            }

            MultiUIAdapter.MovePhaseReadAction();
        }
    }
}