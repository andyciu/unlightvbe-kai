﻿using System;
using System.Numerics;
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
        /// 是否進入勝負判斷模式
        /// </summary>
        private bool IsJudgmentMode { get; set; } = false;
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
        /// <summary>
        /// 玩家雙方場上距離
        /// </summary>
        private PlayerDistanceType PlayerDistance;
        /// <summary>
        /// 對戰每回合攻擊優先方位標記
        /// </summary>
        private UserPlayerType AttackPhaseFirst;
        /// <summary>
        /// 對戰雙方勝負結果
        /// </summary>
        public ShowJudgmentType[] PlayerJudgment { get; private set; } = { ShowJudgmentType.None, ShowJudgmentType.None };
        public static readonly Random Rnd = new(DateTime.Now.Millisecond);


        public BattleSystem(Player player1, Player player2)
        {
            try
            {
                PlayerDatas[(int)UserPlayerType.Player1] = new(player1, UserPlayerType.Player1);
                PlayerDatas[(int)UserPlayerType.Player2] = new(player2, UserPlayerType.Player2);
                if (player1.Deck.Deck_Subs.Count == 1 && player2.Deck.Deck_Subs.Count == 1)
                {
                    PlayerVersusMode = PlayerVersusModeType.OneOnOne;
                }
                else if (player1.Deck.Deck_Subs.Count == 3 && player2.Deck.Deck_Subs.Count == 3)
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
            if (IsFinish) return;

            InitialData();
            StartScreenPhase();
            while (TurnNum <= TurnMaxNum && !IsJudgmentMode)
            {
                if (!IsJudgmentMode) DrawPhase(); else break;
                if (!IsJudgmentMode) MovePhase(); else break;
                if (!IsJudgmentMode) AttackWithDefensePhase(); else break;
            }
            JudgmentPhase();
            IsFinish = true;
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

            foreach (var type in System.Enum.GetValues<CardDeckType>())
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

            PlayerDistance = PlayerDistanceType.Middle;

        }

        /// <summary>
        /// 開始畫面階段
        /// </summary>
        private void StartScreenPhase()
        {
            MultiUIAdapter.ShowStartScreen(new()
            {
                PlayerSelf_Id = PlayerDatas[(int)UserPlayerType.Player1].Player.PlayerId,
                PlayerOpponent_Id = PlayerDatas[(int)UserPlayerType.Player2].Player.PlayerId,
                PlayerSelf_CharacterVBEID = PlayerDatas[(int)UserPlayerType.Player1].CharacterDatas.Select(x => x.Character.VBEID).ToList(),
                PlayerOpponent_CharacterVBEID = PlayerDatas[(int)UserPlayerType.Player2].CharacterDatas.Select(x => x.Character.VBEID).ToList()
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
            MultiUIAdapter.UpdateData_All(new()
            {
                Type = UpdateDataType.TurnNumber,
                Value = TurnNum
            });

            MultiUIAdapter.PhaseStart(new()
            {
                Type = PhaseStartType.Draw
            });

            //若牌堆集合卡牌數量不足時進行洗牌
            if (CardDecks[(int)CardDeckType.Deck].Count < waitToDeal_P1 + waitToDeal_P2)
            {
                GraveyardDeckReUse();
                CardDecks[(int)CardDeckType.Deck] = ShuffleDeck(CardDecks[(int)CardDeckType.Deck]);
            }

            //同步牌堆數量
            MultiUIAdapter.UpdateData_All(new()
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

            //同步牌堆數量
            MultiUIAdapter.UpdateData_All(new()
            {
                Type = UpdateDataType.DeckCount,
                Value = CardDecks[(int)CardDeckType.Deck].Count
            });
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

            MultiUIAdapter.PhaseStart(new()
            {
                Type = PhaseStartType.Move
            });

            MultiUIAdapter.MovePhaseReadAction();

            //回血動作
            foreach (var player in PlayerDatas)
            {
                if (player.MoveBarSelect == MoveBarSelectType.Stay)
                {
                    CharacterHPHeal(player.PlayerType, player.CurrentCharacter.Character.VBEID, 1);
                }
            }

            //加總移動值
            Dictionary<ActionCardType, int>[] cardtotal = new Dictionary<ActionCardType, int>[2];
            int[] movPlayerTotal = new int[2] { 0, 0 };
            int movSystemTotal = 0;

            foreach (var player in System.Enum.GetValues<UserPlayerType>())
            {
                cardtotal[(int)player] = GetCardTotalNumber(player, ActionCardLocation.Play);
                cardtotal[(int)player].TryGetValue(ActionCardType.MOV, out movPlayerTotal[(int)player]);

                switch (PlayerDatas[(int)player].MoveBarSelect)
                {
                    case MoveBarSelectType.Left:
                        movSystemTotal += movPlayerTotal[(int)player];
                        break;
                    case MoveBarSelectType.Right:
                        movSystemTotal -= movPlayerTotal[(int)player];
                        break;
                }
            }

            //計算移動距離
            if (movSystemTotal != 0)
            {
                int newDistance = (int)PlayerDistance + movSystemTotal;
                PlayerDistance = newDistance switch
                {
                    > 2 => PlayerDistanceType.Long,
                    < 0 => PlayerDistanceType.Close,
                    _ => (PlayerDistanceType)newDistance
                };
            }

            //判斷優先權
            if (movPlayerTotal[(int)UserPlayerType.Player1] > movPlayerTotal[(int)UserPlayerType.Player2])
            {
                AttackPhaseFirst = UserPlayerType.Player1;
            }
            else if (movPlayerTotal[(int)UserPlayerType.Player1] < movPlayerTotal[(int)UserPlayerType.Player2])
            {
                AttackPhaseFirst = UserPlayerType.Player2;
            }
            else
            {
                AttackPhaseFirst = (UserPlayerType)Rnd.Next(2);
            }

            MultiUIAdapter.UpdateData_All(new()
            {
                Type = UpdateDataType.PlayerDistanceType,
                Value = (int)PlayerDistance
            });

            MultiUIAdapter.UpdateDataRelative(UpdateDataRelativeType.AttackPhaseFirstPlayerType, AttackPhaseFirst, 0, null);

            MultiUIAdapter.OpenOppenentPlayingCard(
                CardDecks[(int)CardDeckType.Play_P1].Select(x => x.Value).ToList(),
                CardDecks[(int)CardDeckType.Play_P2].Select(x => x.Value).ToList());

            //收牌
            CollectPlayingCardToGraveyard();

            //交換角色動作
            foreach (var player in PlayerDatas)
            {
                if (player.MoveBarSelect == MoveBarSelectType.Change)
                {
                    MultiUIAdapter.ChangeCharacterAction(player.PlayerType);
                }
            }

            //角色存活檢查
            if (!PlayerCharacterHPCheck())
            {
                IsJudgmentMode = true;
            }
        }

        /// <summary>
        /// 攻擊/防禦階段
        /// </summary>
        private void AttackWithDefensePhase()
        {
            UserPlayerType attackPlyaer, defensePlayer;
            int[] diceTrue;
            int diceTrueTotal;

            for (int i = 0; i < 2; i++)
            {
                if (i == 0)
                {
                    attackPlyaer = AttackPhaseFirst;
                    defensePlayer = AttackPhaseFirst == UserPlayerType.Player1 ? UserPlayerType.Player2 : UserPlayerType.Player1;
                }
                else
                {
                    attackPlyaer = AttackPhaseFirst == UserPlayerType.Player1 ? UserPlayerType.Player2 : UserPlayerType.Player1;
                    defensePlayer = AttackPhaseFirst;
                }

                foreach (var player in PlayerDatas)
                {
                    player.DiceTotal = 0;
                    player.IsOKButtonSelect = false;
                }

                MultiUIAdapter.PhaseStartAttackWithDefense(attackPlyaer);

                //Attack Action
                MultiUIAdapter.AttackWithDefensePhaseReadAction(attackPlyaer, PhaseStartType.Attack);
                MultiUIAdapter.OpenOppenentPlayingCard(defensePlayer,
                    CardDecks[(int)GetCardDeckType(attackPlyaer, ActionCardLocation.Play)].Select(x => x.Value).ToList());

                //Defense Action
                MultiUIAdapter.AttackWithDefensePhaseReadAction(defensePlayer, PhaseStartType.Defense);
                MultiUIAdapter.OpenOppenentPlayingCard(attackPlyaer,
                    CardDecks[(int)GetCardDeckType(defensePlayer, ActionCardLocation.Play)].Select(x => x.Value).ToList());

                //骰數再計算
                UpdatePlayerDiceTotalNumber(attackPlyaer, PhaseStartType.Attack);
                UpdatePlayerDiceTotalNumber(defensePlayer, PhaseStartType.Defense);
                MultiUIAdapter.UpdateDiceTotalNumberRelative(attackPlyaer, defensePlayer,
                    PlayerDatas[(int)attackPlyaer].DiceTotal, PlayerDatas[(int)defensePlayer].DiceTotal);


                if (PlayerDatas[(int)attackPlyaer].DiceTotal > 0)
                {
                    MultiUIAdapter.ShowBattleMessage(string.Format("Determines attack power {0}.", PlayerDatas[(int)attackPlyaer].DiceTotal));
                }
                else
                {
                    MultiUIAdapter.ShowBattleMessage("Cancel attack.");
                }

                //收牌
                CollectPlayingCardToGraveyard();

                //擲骰
                diceTrue = new int[2];
                foreach (var player in PlayerDatas)
                {
                    diceTrue[(int)player.PlayerType] = DiceAction(player.DiceTotal);
                }

                MultiUIAdapter.UpdateDiceTrueNumber(diceTrue[(int)UserPlayerType.Player1], diceTrue[(int)UserPlayerType.Player2]);

                //傷害計算
                diceTrueTotal = diceTrue[(int)attackPlyaer] - diceTrue[(int)defensePlayer];

                if (diceTrueTotal > 0)
                {
                    CharacterHPDamage(defensePlayer, PlayerDatas[(int)defensePlayer].CurrentCharacter.Character.VBEID, diceTrueTotal);
                }

                //角色存活檢查
                if (!PlayerCharacterHPCheck())
                {
                    IsJudgmentMode = true;
                    break;
                }
            }
        }

        /// <summary>
        /// 對戰勝負判斷階段
        /// </summary>
        private void JudgmentPhase()
        {
            int[] hpTotal = new int[2];
            foreach (var player in PlayerDatas)
            {
                foreach (var characterData in player.CharacterDatas)
                {
                    if (characterData.CurrentHP > 0)
                    {
                        hpTotal[(int)player.PlayerType] += characterData.CurrentHP;
                    }
                }
            }

            if (hpTotal[(int)UserPlayerType.Player1] > hpTotal[(int)UserPlayerType.Player2])
            {
                PlayerJudgment[(int)UserPlayerType.Player1] = ShowJudgmentType.Victory;
                PlayerJudgment[(int)UserPlayerType.Player2] = ShowJudgmentType.Defeat;
            }
            else if (hpTotal[(int)UserPlayerType.Player1] < hpTotal[(int)UserPlayerType.Player2])
            {
                PlayerJudgment[(int)UserPlayerType.Player1] = ShowJudgmentType.Defeat;
                PlayerJudgment[(int)UserPlayerType.Player2] = ShowJudgmentType.Victory;
            }
            else
            {
                PlayerJudgment[(int)UserPlayerType.Player1] = ShowJudgmentType.Draw;
                PlayerJudgment[(int)UserPlayerType.Player2] = ShowJudgmentType.Draw;
            }
            MultiUIAdapter.ShowJudgment(PlayerJudgment[(int)UserPlayerType.Player1], PlayerJudgment[(int)UserPlayerType.Player2]);
        }
    }
}