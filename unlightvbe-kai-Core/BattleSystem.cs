﻿using unlightvbe_kai_core.Enum;
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
        protected Dictionary<CardDeckType, Dictionary<int, Card>> CardDecks { get; set; }
        /// <summary>
        /// 卡牌集合索引(卡牌編號->對應集合)
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
        /// 攻擊/防禦模式雙方擲骰後正骰數量
        /// </summary>
        private int[] DiceTrue = new int[2];
        /// <summary>
        /// 攻擊/防禦模式擲骰後正骰數量差(攻擊正面骰減去防禦正面骰)
        /// </summary>
        private int DiceTrueTotal;
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
        /// 當前回合階段
        /// </summary>
        private PhaseType[] Phase = new PhaseType[2];
        /// <summary>
        /// 對戰雙方勝負結果
        /// </summary>
        public ShowJudgmentType[] PlayerJudgment { get; private set; } = { ShowJudgmentType.None, ShowJudgmentType.None };
        /// <summary>
        /// 技能執行器
        /// </summary>
        protected SkillAdapterClass SkillAdapter { get; set; }
        /// <summary>
        /// 技能執行指令解釋器
        /// </summary>
        protected SkillCommandProxyClass SkillCommandProxy { get; set; }
        /// <summary>
        /// 初始戰鬥公牌排組
        /// </summary>
        protected List<ActionCard> InitialCardDeck { get; set; }
        private static readonly Random Rnd = new(DateTime.Now.Millisecond);


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

                //設定技能執行介面
                SkillAdapter = new SkillAdapterClass(this);
                SkillCommandProxy = new SkillCommandProxyClass(this);
                SkillAdapter.SetSkillCommandProxy(SkillCommandProxy);
                SkillCommandProxy.SetSkillAdapter(SkillAdapter);
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

        public void SetInitialCardDeck(List<ActionCard> cardlist)
        {
            InitialCardDeck = cardlist;
        }

        public void Start()
        {
            if (IsFinish) return;
            if (MultiUIAdapter == null) throw new Exception("MultiUIAdapter not Set.");
            if (InitialCardDeck == null) throw new Exception("InitialCardDeck not Set.");

            InitialData();
            StartScreenPhase();
            while (TurnNum <= TurnMaxNum && !IsJudgmentMode)
            {
                if (!IsJudgmentMode) DrawPhase(); else break;
                if (!IsJudgmentMode) MovePhase(); else break;
                if (!IsJudgmentMode) AttackWithDefensePhase(); else break;
                if (!IsJudgmentMode) TurnEndPhase(); else break;
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
            CardDecks = new Dictionary<CardDeckType, Dictionary<int, Card>>();
            CardDeckIndex = new Dictionary<int, CardDeckType>();

            foreach (var type in System.Enum.GetValues<CardDeckType>())
            {
                CardDecks.Add(type, new Dictionary<int, Card>());
            }

            ImportActionCardToDeck();
            ImportEventCardToDeck();

            CardDecks[CardDeckType.Deck] = ShuffleDeck(CardDecks[CardDeckType.Deck]);
            CardDecks[CardDeckType.Event_P1] = ShuffleDeck(CardDecks[CardDeckType.Event_P1]);
            CardDecks[CardDeckType.Event_P2] = ShuffleDeck(CardDecks[CardDeckType.Event_P2]);

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
            int waitToDeal_P1 = PlayerDatas[(int)UserPlayerType.Player1].HoldMaxCount - CardDecks[CardDeckType.Hold_P1].Count;
            int waitToDeal_P2 = PlayerDatas[(int)UserPlayerType.Player2].HoldMaxCount - CardDecks[CardDeckType.Hold_P2].Count;
            List<Card> dealCards_p1 = new List<Card>();
            List<Card> dealCards_p2 = new List<Card>();
            ActionCardOwner dealside = ActionCardOwner.Player1; //目前發牌方
            EventCard[] eventCards = new EventCard[2];

            Phase[(int)UserPlayerType.Player1] = PhaseType.Draw;
            Phase[(int)UserPlayerType.Player2] = PhaseType.Draw;

            //回合數增加
            TurnNum += 1;
            MultiUIAdapter.UpdateData_All(new()
            {
                Type = UpdateDataType.TurnNumber,
                Value = TurnNum
            });

            MultiUIAdapter.PhaseStart(new()
            {
                Type = PhaseType.Draw
            });

            //同步牌堆數量
            MultiUIAdapter.UpdateData_All(new()
            {
                Type = UpdateDataType.DeckCount,
                Value = CardDecks[(int)CardDeckType.Deck].Count
            });

            //執行階段(0)
            SkillAdapter.StageStart(0, UserPlayerType.Player1, true, true);

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

                var tmpcard = CardDecks[GetCardDeckType(dealside, ActionCardLocation.Deck)].ElementAt(0).Value;
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

            //執行階段(1)
            SkillAdapter.StageStart(1, UserPlayerType.Player1, true, true);
        }

        /// <summary>
        /// 移動階段
        /// </summary>
        private void MovePhase()
        {
            Phase[(int)UserPlayerType.Player1] = PhaseType.Move;
            Phase[(int)UserPlayerType.Player2] = PhaseType.Move;

            foreach (var player in PlayerDatas)
            {
                player.MoveBarSelect = MoveBarSelectType.None;
                player.IsOKButtonSelect = false;
            }

            MultiUIAdapter.PhaseStart(new()
            {
                Type = PhaseType.Move
            });

            MultiUIAdapter.MovePhaseReadAction();

            //執行階段(2/3/4/70/71)
            foreach (var tmpNum in new int[] { 2, 3, 4, 70, 71 })
            {
                SkillAdapter.StageStart(tmpNum, UserPlayerType.Player1, true, true);
            }

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

            //執行階段(5/6)
            foreach (var tmpNum in new int[] { 5, 6 })
            {
                SkillAdapter.StageStart(tmpNum, AttackPhaseFirst, true, true);
            }

            MultiUIAdapter.OpenOppenentPlayingCard(
                CardDecks[CardDeckType.Play_P1].Select(x => x.Value).ToList(),
                CardDecks[CardDeckType.Play_P2].Select(x => x.Value).ToList());

            //執行階段(7)
            SkillAdapter.StageStart(7, AttackPhaseFirst, true, true);

            //收牌
            CollectPlayingCardToGraveyard();

            //執行階段(8)
            SkillAdapter.StageStart(8, AttackPhaseFirst, true, true);

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
            else
            {
                //執行階段(9)
                SkillAdapter.StageStart(9, AttackPhaseFirst, true, true);
            }
        }

        /// <summary>
        /// 攻擊/防禦階段
        /// </summary>
        private void AttackWithDefensePhase()
        {
            UserPlayerType attackPlyaer, defensePlayer;

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

                Phase[(int)attackPlyaer] = PhaseType.Attack;
                Phase[(int)defensePlayer] = PhaseType.Defense;

                foreach (var player in PlayerDatas)
                {
                    player.DiceTotal = 0;
                    player.IsOKButtonSelect = false;
                }

                MultiUIAdapter.PhaseStartAttackWithDefense(attackPlyaer);

                //執行階段(17/37)
                SkillAdapter.StageStart(17, attackPlyaer, true, false);
                SkillAdapter.StageStart(37, defensePlayer, true, false);

                //Attack Action
                MultiUIAdapter.AttackWithDefensePhaseReadAction(attackPlyaer, PhaseType.Attack);
                MultiUIAdapter.OpenOppenentPlayingCard(defensePlayer,
                    CardDecks[GetCardDeckType(attackPlyaer, ActionCardLocation.Play)].Select(x => x.Value).ToList());

                //Defense Action
                MultiUIAdapter.AttackWithDefensePhaseReadAction(defensePlayer, PhaseType.Defense);
                MultiUIAdapter.OpenOppenentPlayingCard(attackPlyaer,
                    CardDecks[GetCardDeckType(defensePlayer, ActionCardLocation.Play)].Select(x => x.Value).ToList());

                //執行階段(10/30)
                SkillAdapter.StageStart(10, attackPlyaer, true, false);
                SkillAdapter.StageStart(30, defensePlayer, true, false);

                //執行階段(11/31)
                SkillAdapter.StageStart(11, attackPlyaer, true, false);
                SkillAdapter.StageStart(31, defensePlayer, true, false);

                //骰數再計算
                UpdatePlayerDiceTotalNumber(attackPlyaer, PhaseType.Attack);
                //UpdatePlayerDiceTotalNumber(defensePlayer, PhaseType.Defense);
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

                //執行階段(12/32)
                SkillAdapter.StageStart(12, attackPlyaer, true, false);
                SkillAdapter.StageStart(32, defensePlayer, true, false);

                //收牌
                CollectPlayingCardToGraveyard();

                //角色存活檢查
                if (!PlayerCharacterHPCheck())
                {
                    IsJudgmentMode = true;
                    return;
                }

                //執行階段(13/33)
                SkillAdapter.StageStart(13, attackPlyaer, true, false);
                SkillAdapter.StageStart(33, defensePlayer, true, false);

                //擲骰
                DiceTrue = new int[2];
                foreach (var player in PlayerDatas)
                {
                    DiceTrue[(int)player.PlayerType] = DiceAction(player.DiceTotal);
                }

                MultiUIAdapter.UpdateDiceTrueNumber(DiceTrue[(int)UserPlayerType.Player1], DiceTrue[(int)UserPlayerType.Player2]);

                //傷害計算
                DiceTrueTotal = DiceTrue[(int)attackPlyaer] - DiceTrue[(int)defensePlayer];

                //執行階段(20~29)
                foreach (var tmpNum in new int[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 })
                {
                    SkillAdapter.StageStart(tmpNum, attackPlyaer, true, true);
                }

                if (DiceTrueTotal > 0)
                {
                    CharacterHPDamage(defensePlayer, PlayerDatas[(int)defensePlayer].CurrentCharacter.Character.VBEID, DiceTrueTotal);
                }

                //執行階段(14/34)
                SkillAdapter.StageStart(14, attackPlyaer, true, false);
                SkillAdapter.StageStart(34, defensePlayer, true, false);

                //執行階段(15/35)
                SkillAdapter.StageStart(15, attackPlyaer, true, false);
                SkillAdapter.StageStart(35, defensePlayer, true, false);

                //執行階段(16/36)
                SkillAdapter.StageStart(16, attackPlyaer, true, false);
                SkillAdapter.StageStart(36, defensePlayer, true, false);

                //角色存活檢查
                if (!PlayerCharacterHPCheck())
                {
                    IsJudgmentMode = true;
                    return;
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

        /// <summary>
        /// 回合結束階段
        /// </summary>
        private void TurnEndPhase()
        {
            //執行階段(50/51/52)
            foreach (var tmpNum in new int[] { 50, 51, 52 })
            {
                SkillAdapter.StageStart(tmpNum, AttackPhaseFirst.GetOppenentPlayer(), true, true);
            }

            //角色存活檢查
            if (!PlayerCharacterHPCheck())
            {
                IsJudgmentMode = true;
                return;
            }

            int waitToDeal_P1 = PlayerDatas[(int)UserPlayerType.Player1].HoldMaxCount - CardDecks[CardDeckType.Hold_P1].Count;
            int waitToDeal_P2 = PlayerDatas[(int)UserPlayerType.Player2].HoldMaxCount - CardDecks[CardDeckType.Hold_P2].Count;

            //若牌堆集合卡牌數量不足時進行洗牌
            if (CardDecks[CardDeckType.Deck].Count < waitToDeal_P1 + waitToDeal_P2)
            {
                GraveyardDeckReUse();
                CardDecks[CardDeckType.Deck] = ShuffleDeck(CardDecks[CardDeckType.Deck]);
            }

            //執行階段(53/54/55)
            foreach (var tmpNum in new int[] { 53, 54, 55 })
            {
                SkillAdapter.StageStart(tmpNum, AttackPhaseFirst.GetOppenentPlayer(), true, true);
            }

            MultiUIAdapter.ShowBattleMessage(string.Format("Turn {0} end.", TurnNum));
        }
    }
}