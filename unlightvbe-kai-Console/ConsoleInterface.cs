using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_console
{
    public class ConsoleInterface : IUserInterfaceAsync
    {
        protected class CharacterData
        {
            public Character Character { get; set; }
            public int CurrentHP { get; set; }
        }

        protected class PlayerData
        {
            public Player Player { get; set; }
            public List<CharacterData> CharacterDatas { get; set; }
        }

        /// <summary>
        /// 執行期名稱
        /// </summary>
        public string InstanceName { get; set; }
        protected PlayerData[] PlayerDatas { get; set; } = new PlayerData[2];
        /// <summary>
        /// 手牌清單
        /// </summary>
        protected List<CardModel> HoldCards = new();
        /// <summary>
        /// 出牌清單
        /// </summary>
        protected List<CardModel> PlayCards = new();
        /// <summary>
        /// 對手出牌清單
        /// </summary>
        protected List<CardModel> OpponentPlayCards = new();
        /// <summary>
        /// 對手手牌數量
        /// </summary>
        protected int OpponentHoldCardCount;
        /// <summary>
        /// 對手出牌數量
        /// </summary>
        protected int OpponentPlayCardCount;
        /// <summary>
        /// 當前回合數
        /// </summary>
        protected int TurnNum;
        /// <summary>
        /// 牌堆卡牌數
        /// </summary>
        protected int DeckNum;
        /// <summary>
        /// 最後讀取行動資料
        /// </summary>
        protected ReadActionModel? LastReadAction = null;
        /// <summary>
        /// 最後更換角色行動資料
        /// </summary>
        protected ChangeCharacterActionModel? LastChangeCharacterAction = null;
        /// <summary>
        /// 雙方當前總骰數
        /// </summary>
        protected int[] DiceTotalNum = new int[2];
        /// <summary>
        /// 雙方當前擲骰有效數
        /// </summary>
        protected int[] DiceTrueNum = new int[2];
        /// <summary>
        /// 我方移動階段行動選擇
        /// </summary>
        protected MoveBarSelectType MoveBarSelectType;
        /// <summary>
        /// 我方是否已按下OK按鈕
        /// </summary>
        protected bool IsOKButtonClick;
        /// <summary>
        /// 當前回合階段
        /// </summary>
        protected PhaseType PhaseType;
        /// <summary>
        /// 玩家雙方場上距離
        /// </summary>
        protected PlayerDistanceType PlayerDistance = PlayerDistanceType.Middle;
        /// <summary>
        /// 對戰每回合攻擊優先方位標記
        /// </summary>
        protected UserPlayerRelativeType AttackPhaseFirst;
        /// <summary>
        /// 勝負判決紀錄
        /// </summary>
        protected ShowJudgmentType ShowJudgmentType = ShowJudgmentType.None;
        /// <summary>
        /// 對手是否在更換角色中
        /// </summary>
        protected bool IsOpponentCharacterChangeing;
        /// <summary>
        /// 人物主動技能燈開關標記
        /// </summary>
        protected bool[] ActiveSkillLineLight = new bool[4];
        /// <summary>
        /// 人物被動技能燈開關標記
        /// </summary>
        protected bool[,] PassiveSkillLineLight = new bool[2, 4];
        public ConsoleInterface(string instanceName, Player selfPlayer, Player opponentPlayer)
        {
            InstanceName = instanceName;
            PlayerDatas[(int)UserPlayerRelativeType.Self] = new()
            {
                Player = selfPlayer,
                CharacterDatas = selfPlayer.Deck.Deck_Subs.Select(x => new CharacterData()
                {
                    Character = x.character,
                    CurrentHP = x.character.HP
                }).ToList(),
            };
            PlayerDatas[(int)UserPlayerRelativeType.Opponent] = new()
            {
                Player = opponentPlayer,
                CharacterDatas = opponentPlayer.Deck.Deck_Subs.Select(x => new CharacterData()
                {
                    Character = x.character,
                    CurrentHP = x.character.HP
                }).ToList(),
            };
        }

        protected virtual string ShowInstanceName()
        {
            return "[" + InstanceName + "]";
        }

        protected virtual void SetConsoleColor()
        {
            Console.ResetColor();
        }

        protected void ConsoleWriteLine(string message)
        {
            lock (Console.Out)
            {
                SetConsoleColor();
                Console.WriteLine(ShowInstanceName() + message);
            }
        }

        protected void ConsoleWriteLine(string message, params object?[]? arg)
        {
            lock (Console.Out)
            {
                SetConsoleColor();
                Console.WriteLine(ShowInstanceName() + message, arg);
            }
        }

        protected void ConsoleWrite(string message)
        {
            lock (Console.Out)
            {
                SetConsoleColor();
                Console.Write(ShowInstanceName() + message);
            }
        }

        public void ShowStartScreen(ShowStartScreenModel data)
        {
            ConsoleWriteLine("Test_ShowStartScreen");
            ConsoleWriteLine("PlayerSelf_Id=" + data.PlayerSelf_Id);

            Thread.Sleep(1000);

            ConsoleWriteLine("PlayerOpponent_Id=" + data.PlayerOpponent_Id);

            if (PlayerDatas[(int)UserPlayerRelativeType.Self].Player.PlayerId != data.PlayerSelf_Id)
            {
                ConsoleWriteLine("PlayerSelf_Id no match.");
            }
            if (PlayerDatas[(int)UserPlayerRelativeType.Opponent].Player.PlayerId != data.PlayerOpponent_Id)
            {
                ConsoleWriteLine("PlayerOpponent_Id no match.");
            }

            for (int i = 0; i < PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Count; i++)
            {
                if (PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas[i].Character.VBEID != data.PlayerSelf_CharacterVBEID[i])
                {
                    ConsoleWriteLine("PlayerSelf CharacterData[{0}] VBEID no match.", i);
                }
            }
            for (int i = 0; i < PlayerDatas[(int)UserPlayerRelativeType.Opponent].CharacterDatas.Count; i++)
            {
                if (PlayerDatas[(int)UserPlayerRelativeType.Opponent].CharacterDatas[i].Character.VBEID != data.PlayerOpponent_CharacterVBEID[i])
                {
                    ConsoleWriteLine("PlayerSelf CharacterData[{0}] VBEID no match.", i);
                }
                break; // PlayerOpponent_CharacterVBEID count only one
            }
            for (int i = 0; i < data.PlayerSelf_CharacterVBEID.Count; i++)
            {
                ConsoleWriteLine("PlayerSelf_CharacterVBEID[{0}]={1}", i + 1, data.PlayerSelf_CharacterVBEID[i]);
            }
            for (int i = 0; i < data.PlayerOpponent_CharacterVBEID.Count; i++)
            {
                ConsoleWriteLine("PlayerOpponent_CharacterVBEID[{0}]={1}", i + 1, data.PlayerOpponent_CharacterVBEID[i]);
            }
        }

        public void ShowBattleMessage(string message)
        {
            ConsoleWriteLine("Test_ShowBattleMessage");
            ConsoleWriteLine(message);
        }
        public virtual ReadActionModel ReadAction()
        {
            ReadActionModel? readActionModel = null;

            while (readActionModel == null && !IsOKButtonClick)
            {
                string tmpstr = string.Empty;
                ConsoleWriteLine("=======================");
                while (string.IsNullOrEmpty(tmpstr))
                {
                    ConsoleWrite("Action(\"Type Message\"): ");
                    tmpstr = Console.ReadLine();
                }

                var tmparr = tmpstr.Split(' ');

                switch (tmparr[0].ToUpper())
                {
                    case "CC":
                        if (tmparr.Length < 2) continue;

                        readActionModel = new()
                        {
                            Type = UserActionType.CardClick,
                            Value = int.Parse(tmparr[1])
                        };
                        break;
                    case "CR":
                        if (tmparr.Length < 2) continue;

                        readActionModel = new()
                        {
                            Type = UserActionType.CardReverse,
                            Value = int.Parse(tmparr[1])
                        };
                        break;
                    case "BARL":
                        readActionModel = new()
                        {
                            Type = UserActionType.BarMoveLeft
                        };
                        break;
                    case "BARR":
                        readActionModel = new()
                        {
                            Type = UserActionType.BarMoveRight
                        };
                        break;
                    case "BARS":
                        readActionModel = new()
                        {
                            Type = UserActionType.BarMoveStay
                        };
                        break;
                    case "BARC":
                        readActionModel = new()
                        {
                            Type = UserActionType.BarMoveChange
                        };
                        break;
                    case "OK":
                        readActionModel = new()
                        {
                            Type = UserActionType.OKButtonClick
                        };
                        break;
                    case "SC": //ShowCard
                        ConsoleWriteLine("HoldCards:");
                        foreach (var card in HoldCards)
                        {
                            ConsoleWriteLine("({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
                        }
                        ConsoleWriteLine("PlayCards:");
                        foreach (var card in PlayCards)
                        {
                            ConsoleWriteLine("({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
                        }
                        break;
                    case "SI": //ShowInfo
                        ConsoleWriteLine("Turn=" + TurnNum);
                        ConsoleWriteLine("Phase=" + PhaseType.ToString());
                        ConsoleWriteLine("Distance=" + PlayerDistance.ToString());
                        ConsoleWriteLine("Deck=" + DeckNum);
                        ConsoleWriteLine("SelfHold=" + HoldCards.Count);
                        ConsoleWriteLine("SelfPlay=" + PlayCards.Count);
                        ConsoleWriteLine("OpponentHold=" + OpponentHoldCardCount);
                        ConsoleWriteLine("OpponentPlay=" + OpponentPlayCardCount);
                        if (PhaseType == PhaseType.Attack || PhaseType == PhaseType.Defense)
                        {
                            ConsoleWriteLine("SelfDiceTotal=" + DiceTotalNum[(int)UserPlayerRelativeType.Self].ToString());
                            ConsoleWriteLine("OpponentDiceTotal=" + DiceTotalNum[(int)UserPlayerRelativeType.Opponent].ToString());
                        }
                        break;
                    case "SA": //ShowCharacter
                        foreach (var playertype in Enum.GetValues<UserPlayerRelativeType>())
                        {
                            int tmpnum = 0;
                            foreach (var characterdata in PlayerDatas[(int)playertype].CharacterDatas)
                            {
                                ConsoleWriteLine("{0}[{1}] = {2} #{3} ({{{4}/{5}}}/{6}/{7})", playertype.ToString(), tmpnum++, characterdata.Character.Name,
                                    characterdata.Character.VBEID, characterdata.CurrentHP, characterdata.Character.HP, characterdata.Character.ATK, characterdata.Character.DEF);
                            }
                        }
                        break;
                    case "HELP":
                        ConsoleWriteLine("Command list(Case-insensitive):");
                        ConsoleWriteLine("CC [card Number] - CardClick");
                        ConsoleWriteLine("CR [card Number] - CardReverse");
                        ConsoleWriteLine("BARL - BarMoveLeft");
                        ConsoleWriteLine("BARR - BarMoveRight");
                        ConsoleWriteLine("BARS - BarMoveStay");
                        ConsoleWriteLine("BARC - BarMoveStay");
                        ConsoleWriteLine("OK - OKButtonClick");
                        ConsoleWriteLine("SC - ShowInfo(Card)");
                        ConsoleWriteLine("SI - ShowInfo(Battle)");
                        ConsoleWriteLine("SA - ShowInfo(Character)");
                        ConsoleWriteLine("HELP - Show command list");
                        break;
                    default:
                        ConsoleWriteLine("Action Unknown. Please try again.");
                        break;
                }
            }

            if (IsOKButtonClick) ConsoleWriteLine("ReadAction-OKButtonClicked.");

            LastReadAction = readActionModel;
            return readActionModel!;
        }
        public Task ShowStartScreenAsync(ShowStartScreenModel data)
        {
            ShowStartScreen(data);
            return Task.CompletedTask;
        }

        public Task ShowBattleMessageAsync(string message)
        {
            ShowBattleMessage(message);
            return Task.CompletedTask;
        }

        public Task DrawActionCardAsync(DrawCardModel data)
        {
            DrawActionCard(data);
            return Task.CompletedTask;
        }

        public void DrawActionCard(DrawCardModel data)
        {
            ConsoleWriteLine("Test_DrawActionCard");

            foreach (var card in data.SelfCards)
            {
                HoldCards.Add(card);
                ConsoleWriteLine("({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
                DeckNum--;
            }

            OpponentHoldCardCount += data.OpponentCardCount;
            DeckNum -= data.OpponentCardCount;
            ConsoleWriteLine("OpponentHoldCardCount=" + OpponentHoldCardCount);
        }

        public Task UpdateDataAsync(UpdateDataModel data)
        {
            UpdateData(data);
            return Task.CompletedTask;
        }

        public void UpdateData(UpdateDataModel data)
        {
            ConsoleWriteLine("Test_UpdateData");
            ConsoleWriteLine(data.Type.ToString() + ":" + data.Value + ":" + data.Message);

            CharacterData? characterData = null;

            switch (data.Type)
            {
                case UpdateDataType.TurnNumber:
                    TurnNum = data.Value;
                    break;
                case UpdateDataType.DeckCount:
                    DeckNum = data.Value;
                    break;
                case UpdateDataType.OpponentHoldingCardCount:
                    OpponentHoldCardCount += data.Value;
                    OpponentPlayCardCount -= data.Value;
                    break;
                case UpdateDataType.OpponentPlayingCardCount:
                    OpponentHoldCardCount -= data.Value;
                    OpponentPlayCardCount += data.Value;
                    break;
                case UpdateDataType.PlayerDistanceType:
                    PlayerDistance = (PlayerDistanceType)data.Value;
                    break;
                case UpdateDataType.OKButtonOpen:
                    IsOKButtonClick = false;
                    break;
                case UpdateDataType.OpponentCharacterChangeBegin:
                    IsOpponentCharacterChangeing = true;
                    Task.Run(() =>
                    {
                        ConsoleWrite("Opponent Character Changeing");
                        while (IsOpponentCharacterChangeing)
                        {
                            ConsoleWrite(".");
                            Thread.Sleep(1000);
                        }
                    });
                    break;
                case UpdateDataType.OpponentCharacterChangeAction:
                    if (!string.IsNullOrEmpty(data.Message))
                    {
                        characterData = PlayerDatas[(int)UserPlayerRelativeType.Opponent].CharacterDatas.Find(x => x.Character.VBEID == data.Message!);
                        if (characterData != null)
                        {
                            PlayerDatas[(int)UserPlayerRelativeType.Opponent].CharacterDatas.Remove(characterData);
                            PlayerDatas[(int)UserPlayerRelativeType.Opponent].CharacterDatas.Insert(0, characterData);
                        }
                        else
                        {
                            ConsoleWriteLine("OpponentCharacterChangeAction VBEID not found.");
                        }
                    }

                    break;
                case UpdateDataType.ActiveSkillLineLight:
                    ActiveSkillLineLight[data.Value] = Convert.ToBoolean(data.Message);
                    break;
                default:
                    ConsoleWriteLine("UpdateData Type not found.");
                    break;
            }
        }

        public Task DrawEventCardAsync(DrawCardModel data)
        {
            DrawEventCard(data);
            return Task.CompletedTask;
        }

        public void DrawEventCard(DrawCardModel data)
        {
            ConsoleWriteLine("Test_DrawEventCard");

            foreach (var card in data.SelfCards)
            {
                HoldCards.Add(card);
                ConsoleWriteLine("({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
            }

            OpponentHoldCardCount += data.OpponentCardCount;
            ConsoleWriteLine("OpponentHoldCardCount=" + OpponentHoldCardCount);
        }

        private void CardReverse(CardModel card)
        {
            var tmpType = card.UpperType;
            var tmpNum = card.UpperNum;

            card.UpperType = card.LowerType;
            card.UpperNum = card.LowerNum;

            card.LowerType = tmpType;
            card.LowerNum = tmpNum;
        }

        public Task ReadActionReceiveAsync(ReadActionReceiveModel data)
        {
            ReadActionReceive(data);
            return Task.CompletedTask;
        }

        public void ReadActionReceive(ReadActionReceiveModel data)
        {
            ConsoleWriteLine("Test_ReadActionReceive");
            ConsoleWriteLine(data.Type.ToString());

            CharacterData? characterData = null;

            if (!data.IsSuccess)
            {
                ConsoleWriteLine("IsSuccess=" + data.IsSuccess.ToString());
                return;
            }

            switch (data.Type)
            {
                case ReadActionReceiveType.HoldingCard:
                    if (LastReadAction != null && LastReadAction.Type == UserActionType.CardClick)
                    {
                        var tmpcard = PlayCards.Where(n => n.Number == LastReadAction.Value).First();
                        HoldCards.Add(tmpcard);
                        PlayCards.Remove(tmpcard);
                    }
                    break;
                case ReadActionReceiveType.PlayingCard:
                    if (LastReadAction != null && LastReadAction.Type == UserActionType.CardClick)
                    {
                        var tmpcard = HoldCards.Where(n => n.Number == LastReadAction.Value).First();
                        PlayCards.Add(tmpcard);
                        HoldCards.Remove(tmpcard);
                    }
                    break;
                case ReadActionReceiveType.HoldingCardReverse:
                    if (LastReadAction != null && LastReadAction.Type == UserActionType.CardReverse)
                    {
                        var tmpcard = HoldCards.Where(n => n.Number == LastReadAction.Value).First();
                        CardReverse(tmpcard);
                    }
                    break;
                case ReadActionReceiveType.PlayingCardReverse:
                    if (LastReadAction != null && LastReadAction.Type == UserActionType.CardReverse)
                    {
                        var tmpcard = PlayCards.Where(n => n.Number == LastReadAction.Value).First();
                        CardReverse(tmpcard);
                    }
                    break;
                case ReadActionReceiveType.BarMoveLeft:
                    MoveBarSelectType = MoveBarSelectType.Left;
                    break;
                case ReadActionReceiveType.BarMoveRight:
                    MoveBarSelectType = MoveBarSelectType.Right;
                    break;
                case ReadActionReceiveType.BarMoveStay:
                    MoveBarSelectType = MoveBarSelectType.Stay;
                    break;
                case ReadActionReceiveType.BarMoveChange:
                    MoveBarSelectType = MoveBarSelectType.Change;
                    break;
                case ReadActionReceiveType.OKButtonClick:
                    IsOKButtonClick = true;
                    break;
                case ReadActionReceiveType.ChangeCharacter:
                    if (LastChangeCharacterAction != null)
                    {
                        characterData = PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Find(x => x.Character.VBEID == LastChangeCharacterAction.NewCharacterVBEID);
                        if (characterData != null)
                        {
                            PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Remove(characterData);
                            PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Insert(0, characterData);
                            ConsoleWriteLine("ChangeCharacter:" + characterData.Character.VBEID);
                        }
                        else
                        {
                            ConsoleWriteLine("ChangeCharacter VBEID not found.");
                        }
                    }
                    break;
                default:
                    ConsoleWrite("ReadActionReceiveType not found.");
                    break;
            }
        }

        public Task PhaseStartAsync(PhaseStartModel data)
        {
            PhaseStart(data);
            return Task.CompletedTask;
        }

        public void PhaseStart(PhaseStartModel data)
        {
            ConsoleWriteLine("Test_PhaseStart");
            ConsoleWriteLine(data.Type.ToString());

            PhaseType = data.Type;
        }

        public Task UpdateDataMultiAsync(UpdateDataMultiModel data)
        {
            UpdateDataMulti(data);
            return Task.CompletedTask;
        }

        public void UpdateDataMulti(UpdateDataMultiModel data)
        {
            ConsoleWriteLine("Test_UpdateDataMulti");

            switch (data.Type)
            {
                case UpdateDataMultiType.DiceTotal:
                    ConsoleWriteLine("SelfDiceTotal=" + data.Self.ToString());
                    ConsoleWriteLine("OpponentDiceTotal=" + data.Opponent.ToString());

                    DiceTotalNum[(int)UserPlayerRelativeType.Self] = data.Self;
                    DiceTotalNum[(int)UserPlayerRelativeType.Opponent] = data.Opponent;

                    break;
                case UpdateDataMultiType.PlayedCardCollectCount:
                    ConsoleWriteLine("SelfPlayedCardCollectCount=" + data.Self.ToString());
                    ConsoleWriteLine("OpponentPlayedCardCollectCount=" + data.Opponent.ToString());

                    if (PlayCards.Count != data.Self) ConsoleWriteLine("SelfPlayedCardCollectCount no match.");
                    if (OpponentPlayCardCount != data.Opponent) ConsoleWriteLine("OpponentPlayedCardCollectCount no match.");

                    PlayCards.Clear();
                    OpponentPlayCardCount = 0;

                    break;
                case UpdateDataMultiType.DiceTrue:
                    ConsoleWriteLine("SelfDiceTrue=" + data.Self.ToString());
                    ConsoleWriteLine("OpponentDiceTrue=" + data.Opponent.ToString());

                    DiceTrueNum[(int)UserPlayerRelativeType.Self] = data.Self;
                    DiceTrueNum[(int)UserPlayerRelativeType.Opponent] = data.Opponent;
                    break;
                default:
                    ConsoleWrite("UpdateDataMultiType not found.");
                    break;
            }
        }

        public Task OpenOppenentPlayingCardAsync(OpenOppenentPlayingCardModel data)
        {
            OpenOppenentPlayingCard(data);
            return Task.CompletedTask;
        }

        public void OpenOppenentPlayingCard(OpenOppenentPlayingCardModel data)
        {
            ConsoleWriteLine("Test_OpenOppenentPlayingCard");

            foreach (var card in data.Cards)
            {
                OpponentPlayCards.Add(card);
                ConsoleWriteLine("({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
            }

            if (OpponentPlayCardCount != data.Cards.Count) ConsoleWriteLine("OpponentPlayedCardCollectCount no match.");
        }

        public Task ShowJudgmentAsync(ShowJudgmentModel data)
        {
            ShowJudgment(data);
            return Task.CompletedTask;
        }

        public void ShowJudgment(ShowJudgmentModel data)
        {
            ConsoleWriteLine("Test_ShowJudgment");
            ConsoleWriteLine(data.Type.ToString());

            ShowJudgmentType = data.Type;
        }

        public ChangeCharacterActionModel ChangeCharacterAction()
        {
            ChangeCharacterActionModel? changeCharacterActionModel = null;

            while (changeCharacterActionModel == null)
            {
                string tmpstr = string.Empty;
                int tmpnum = 0;

                ConsoleWriteLine("ChangeCharacterAction");

                foreach (var characterdata in PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas)
                {
                    if (tmpnum == 0)
                    {
                        tmpnum++;
                        continue;
                    }
                    ConsoleWriteLine("Self[{1}]={2}({3}/{4}/{5}/{6})", tmpnum++, characterdata.Character.Name,
                        characterdata.Character.VBEID, characterdata.CurrentHP, characterdata.Character.ATK, characterdata.Character.DEF);
                }
                while (string.IsNullOrEmpty(tmpstr))
                {
                    ConsoleWrite("Type new go on stage character VBEID:");
                    tmpstr = Console.ReadLine();
                }

                if (PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Any(x => x.Character.VBEID == tmpstr))
                {
                    if (tmpstr != PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas[0].Character.VBEID)
                    {
                        changeCharacterActionModel = new()
                        {
                            NewCharacterVBEID = tmpstr,
                        };
                        break;
                    }
                    else
                    {
                        ConsoleWriteLine("Character on stage cannot be assigned.");
                    }
                }
            }

            LastChangeCharacterAction = changeCharacterActionModel;
            return changeCharacterActionModel;
        }

        public Task UpdateDataRelativeAsync(UpdateDataRelativeModel data)
        {
            UpdateDataRelative(data);
            return Task.CompletedTask;
        }

        public void UpdateDataRelative(UpdateDataRelativeModel data)
        {
            ConsoleWriteLine("Test_UpdateDataRelative");
            ConsoleWriteLine(data.Player.ToString());
            ConsoleWriteLine(data.Type.ToString());

            CharacterData? characterData = null;
            switch (data.Type)
            {
                case UpdateDataRelativeType.AttackPhaseFirstPlayerType:
                    AttackPhaseFirst = data.Player;
                    break;
                case UpdateDataRelativeType.CharacterHPDamage:
                    characterData = PlayerDatas[(int)data.Player].CharacterDatas.Find(x => x.Character.VBEID == data.Message);
                    if (characterData != null)
                    {
                        characterData.CurrentHP -= data.Value;
                        if (characterData.CurrentHP < 0) characterData.CurrentHP = 0;

                        ConsoleWriteLine("{0} take {1} damage.", data.Player.ToString(), data.Value);
                    }
                    else
                    {
                        ConsoleWriteLine("CharacterHPDamage CharacterData not found.");
                    }
                    break;
                case UpdateDataRelativeType.CharacterHPHeal:
                    characterData = PlayerDatas[(int)data.Player].CharacterDatas.Find(x => x.Character.VBEID == data.Message);
                    if (characterData != null)
                    {
                        characterData.CurrentHP += data.Value;
                        if (characterData.CurrentHP > characterData.Character.HP) characterData.CurrentHP = characterData.Character.HP;

                        ConsoleWriteLine("{0} restore {1} HP.", data.Player.ToString(), data.Value);
                    }
                    else
                    {
                        ConsoleWriteLine("CharacterHPHeal CharacterData not found.");
                    }
                    break;
                case UpdateDataRelativeType.PassiveSkillLineLight:
                    PassiveSkillLineLight[(int)data.Player, data.Value] = Convert.ToBoolean(data.Message);
                    break;
                default:
                    ConsoleWriteLine("UpdateDataRelativeType not found.");
                    break;
            }
        }

        public Task ShowSkillAnimateAsync(ShowSkillAnimateModel data)
        {
            ShowSkillAnimate(data);
            return Task.CompletedTask;
        }

        public void ShowSkillAnimate(ShowSkillAnimateModel data)
        {
            ConsoleWriteLine("Test_ShowActiveSkillAnimate");
            ConsoleWriteLine("Player: {0}, SkillID: {1}", data.Player.ToString(), data.SkillID);
            Thread.Sleep(1000);
        }
    }
}
