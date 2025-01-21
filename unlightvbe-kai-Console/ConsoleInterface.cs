using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Enum.UserActionProxy;
using unlightvbe_kai_core.Enum.UserInterface;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models.UserInterface;

namespace unlightvbe_kai_console
{
    public class ConsoleInterface : IUserInterface
    {
        public class PlayerModel
        {
            public required int PlayerId { get; set; }
            public required string Name { get; set; }
            public required List<CharacterModel> Characters { get; set; }
        }
        public class CharacterModel
        {
            public required string Name { get; init; }
            public string Title { get; init; } = string.Empty;
            public required int HP { get; init; }
            public required int ATK { get; init; }
            public required int DEF { get; init; }
            public required string VBEID { get; init; }
            public required string EventColour { get; init; }
            public required string LevelMain { get; init; }
            public required int LevelNum { get; init; }
        }

        protected class CharacterData
        {
            public required CharacterModel Character { get; set; }
            public int CurrentHP { get; set; }
            public List<BuffData> BuffDatas { get; set; } = [];
        }

        protected class PlayerData
        {
            public required PlayerModel Player { get; set; }
            public required List<CharacterData> CharacterDatas { get; set; }
        }

        protected class BuffData
        {
            public required string Identifier { get; set; }
            public int Value { get; set; }
            public int Total { get; set; }
        }

        protected class CardData
        {
            public required int Number { get; set; }
            public required ActionCardType UpperType { get; set; }
            public required int UpperNum { get; set; }
            public required ActionCardType LowerType { get; set; }
            public required int LowerNum { get; set; }
            public required ActionCardRelativeOwner Owner { get; set; }
            public required ActionCardLocation Location { get; set; }
            public required string Identifier { get; init; }
            public required bool IsReverse { get; set; }
        }

        /// <summary>
        /// 執行期名稱
        /// </summary>
        public string InstanceName { get; init; }
        protected PlayerData[] PlayerDatas { get; set; } = new PlayerData[2];
        /// <summary>
        /// 手牌清單
        /// </summary>
        protected List<CardData> HoldCards = [];
        /// <summary>
        /// 出牌清單
        /// </summary>
        protected List<CardData> PlayCards = [];
        /// <summary>
        /// 對手出牌清單
        /// </summary>
        protected List<CardData> OpponentPlayCards = [];
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
        protected CommandPlayerDistanceType PlayerDistance = CommandPlayerDistanceType.Middle;
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
        /// <summary>
        /// 讀取指令動作是否正在進行Card Click &amp; Reverse組合動作
        /// </summary>
        protected bool ReadActionIsOnCCR;
        protected Dictionary<string, string> BuffNameDict = [];
        public ConsoleInterface(string instanceName, PlayerModel selfPlayer, PlayerModel opponentPlayer, Dictionary<string, string> buffNameDict)
        {
            InstanceName = instanceName;
            PlayerDatas[(int)UserPlayerRelativeType.Self] = new()
            {
                Player = selfPlayer,
                CharacterDatas = selfPlayer.Characters.Select(x => new CharacterData()
                {
                    Character = x,
                    CurrentHP = x.HP
                }).ToList(),
            };
            PlayerDatas[(int)UserPlayerRelativeType.Opponent] = new()
            {
                Player = opponentPlayer,
                CharacterDatas = opponentPlayer.Characters.Select(x => new CharacterData()
                {
                    Character = x,
                    CurrentHP = x.HP
                }).ToList(),
            };

            BuffNameDict = buffNameDict;
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

        protected void ConsoleWrite(string message, params object?[]? arg)
        {
            lock (Console.Out)
            {
                SetConsoleColor();
                Console.Write(ShowInstanceName() + message, arg);
            }
        }

        public void ShowStartScreen(ShowStartScreenModel data)
        {
            ConsoleWriteLine("Test_ShowStartScreen");
            ConsoleWriteLine("PlayerSelf_Id=" + data.PlayerSelfId);

            Thread.Sleep(1000);

            ConsoleWriteLine("PlayerOpponent_Id=" + data.PlayerOpponentId);

            if (PlayerDatas[(int)UserPlayerRelativeType.Self].Player.PlayerId != data.PlayerSelfId)
            {
                ConsoleWriteLine("PlayerSelf_Id no match.");
            }
            if (PlayerDatas[(int)UserPlayerRelativeType.Opponent].Player.PlayerId != data.PlayerOpponentId)
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
                if (ReadActionIsOnCCR)
                {
                    ReadActionIsOnCCR = false;

                    readActionModel = new()
                    {
                        Type = UserActionType.CardReverse,
                        Value = LastReadAction!.Value
                    };
                    break;
                }

                string? tmpstr = string.Empty;
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
                    case "CCR":
                        if (tmparr.Length < 2) continue;

                        ReadActionIsOnCCR = true;

                        readActionModel = new()
                        {
                            Type = UserActionType.CardClick,
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
                    case "SF": //ShowBuff
                        ConsoleWriteLine("Buff:");
                        foreach (var playertype in Enum.GetValues<UserPlayerRelativeType>())
                        {
                            int tmpnum = 0;
                            foreach (var characterdata in PlayerDatas[(int)playertype].CharacterDatas)
                            {
                                var tmpStr = string.Format("{0}[{1}] = {2} #{3}: ", playertype.ToString(), tmpnum++, characterdata.Character.Name, characterdata.Character.VBEID);
                                if (characterdata.BuffDatas.Count != 0)
                                {
                                    ConsoleWriteLine(tmpStr);
                                    foreach (var buffData in characterdata.BuffDatas)
                                    {
                                        if (BuffNameDict.TryGetValue(buffData.Identifier, out var tmpBuffName))
                                        {
                                            ConsoleWriteLine("[{0} #{1}]{2}/{3}", tmpBuffName, buffData.Identifier, buffData.Value, buffData.Total);
                                        }
                                        else
                                        {
                                            ConsoleWriteLine("[{0}]{1}/{2}", buffData.Identifier, buffData.Value, buffData.Total);
                                        }
                                    }
                                    ConsoleWriteLine("");
                                }
                                else
                                {
                                    ConsoleWriteLine(tmpStr + "[NoBuff]");
                                }
                            }
                        }
                        break;
                    case "HELP":
                        ConsoleWriteLine("Command list(Case-insensitive):");
                        ConsoleWriteLine("CC [card Number] - CardClick");
                        ConsoleWriteLine("CR [card Number] - CardReverse");
                        ConsoleWriteLine("CCR [card Number] - Card Click&Reverse");
                        ConsoleWriteLine("BARL - BarMoveLeft");
                        ConsoleWriteLine("BARR - BarMoveRight");
                        ConsoleWriteLine("BARS - BarMoveStay");
                        ConsoleWriteLine("BARC - BarMoveChange");
                        ConsoleWriteLine("OK - OKButtonClick");
                        ConsoleWriteLine("SC - ShowInfo(Card)");
                        ConsoleWriteLine("SI - ShowInfo(Battle)");
                        ConsoleWriteLine("SA - ShowInfo(Character)");
                        ConsoleWriteLine("SF - ShowInfo(Buff)");
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

        public void DrawActionCard(DrawCardModel data)
        {
            ConsoleWriteLine("Test_DrawActionCard");

            foreach (var card in data.SelfCards)
            {
                HoldCards.Add(new()
                {
                    Number = card.Number,
                    UpperType = card.UpperType,
                    UpperNum = card.UpperNum,
                    LowerType = card.LowerType,
                    LowerNum = card.LowerNum,
                    Owner = card.Owner,
                    Location = card.Location,
                    Identifier = card.Identifier,
                    IsReverse = card.IsReverse,
                });
                ConsoleWriteLine("({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
                DeckNum--;
            }

            OpponentHoldCardCount += data.OpponentCardCount;
            DeckNum -= data.OpponentCardCount;
            ConsoleWriteLine("OpponentHoldCardCount=" + OpponentHoldCardCount);
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
                    PlayerDistance = (CommandPlayerDistanceType)data.Value;
                    break;
                case UpdateDataType.OKButtonOpen:
                    IsOKButtonClick = false;
                    break;
                case UpdateDataType.OpponentCharacterChangeBegin:
                    IsOpponentCharacterChangeing = true;
                    Task.Run(() =>
                    {
                        ConsoleWrite("Opponent Character Changeing...");
                        while (IsOpponentCharacterChangeing)
                        {
                            Thread.Sleep(1000);
                        }
                    });
                    break;
                case UpdateDataType.OpponentCharacterChangeAction:
                    if (data.Value == 1 && !string.IsNullOrEmpty(data.Message))
                    {
                        characterData = PlayerDatas[(int)UserPlayerRelativeType.Opponent].CharacterDatas.Find(x => x.Character.VBEID == data.Message!);
                        if (characterData != null)
                        {
                            PlayerDatas[(int)UserPlayerRelativeType.Opponent].CharacterDatas.Remove(characterData);
                            PlayerDatas[(int)UserPlayerRelativeType.Opponent].CharacterDatas.Insert(0, characterData);
                            IsOpponentCharacterChangeing = false;
                        }
                        else
                        {
                            ConsoleWriteLine("OpponentCharacterChangeAction VBEID not found.");
                        }
                    }
                    else
                    {
                        ConsoleWriteLine("OpponentCharacterChangeAction Canceled.");
                    }

                    break;
                case UpdateDataType.ActiveSkillLineLight:
                    ActiveSkillLineLight[data.Value] = Convert.ToBoolean(data.Message);
                    break;
                case UpdateDataType.SelfCharacterChangeRandomAction:
                    if (!string.IsNullOrEmpty(data.Message))
                    {
                        characterData = PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Find(x => x.Character.VBEID == data.Message!);
                        if (characterData != null)
                        {
                            PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Remove(characterData);
                            PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Insert(0, characterData);
                        }
                        else
                        {
                            ConsoleWriteLine("SelfCharacterChangeRandomAction VBEID not found.");
                        }
                    }

                    break;
                default:
                    ConsoleWriteLine("UpdateData Type not found.");
                    break;
            }
        }

        public void DrawEventCard(DrawCardModel data)
        {
            ConsoleWriteLine("Test_DrawEventCard");

            foreach (var card in data.SelfCards)
            {
                HoldCards.Add(new()
                {
                    Number = card.Number,
                    UpperType = card.UpperType,
                    UpperNum = card.UpperNum,
                    LowerType = card.LowerType,
                    LowerNum = card.LowerNum,
                    Owner = card.Owner,
                    Location = card.Location,
                    Identifier = card.Identifier,
                    IsReverse = card.IsReverse,
                });
                ConsoleWriteLine("({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
            }

            OpponentHoldCardCount += data.OpponentCardCount;
            ConsoleWriteLine("OpponentHoldCardCount=" + OpponentHoldCardCount);
        }

        private static void CardReverse(CardData card)
        {
            var tmpType = card.UpperType;
            var tmpNum = card.UpperNum;

            card.UpperType = card.LowerType;
            card.UpperNum = card.LowerNum;

            card.LowerType = tmpType;
            card.LowerNum = tmpNum;

            card.IsReverse = !card.IsReverse;
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
                case ReadActionReceiveType.ChangeCharacterCancel:
                    ConsoleWriteLine("ChangeCharacter Action Canceled.");
                    break;
                default:
                    ConsoleWrite("ReadActionReceiveType not found.");
                    break;
            }
        }

        public void PhaseStart(PhaseStartModel data)
        {
            ConsoleWriteLine("Test_PhaseStart");
            ConsoleWriteLine(data.Type.ToString());

            PhaseType = data.Type;
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
                default:
                    ConsoleWrite("UpdateDataMultiType not found.");
                    break;
            }
        }

        public void OpenOppenentPlayingCard(OpenOppenentPlayingCardModel data)
        {
            ConsoleWriteLine("Test_OpenOppenentPlayingCard");

            foreach (var card in data.Cards)
            {
                OpponentPlayCards.Add(new()
                {
                    Number = card.Number,
                    UpperType = card.UpperType,
                    UpperNum = card.UpperNum,
                    LowerType = card.LowerType,
                    LowerNum = card.LowerNum,
                    Owner = card.Owner,
                    Location = card.Location,
                    Identifier = card.Identifier,
                    IsReverse = card.IsReverse,
                });
                ConsoleWriteLine("({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
            }

            if (OpponentPlayCardCount != data.Cards.Count) ConsoleWriteLine("OpponentPlayedCardCollectCount no match.");
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
                string? tmpstr = string.Empty;
                int tmpnum = 0;

                ConsoleWriteLine("ChangeCharacterAction");

                foreach (var characterdata in PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas)
                {
                    if (tmpnum == 0)
                    {
                        tmpnum++;
                        continue;
                    }
                    ConsoleWriteLine("Self[{0}] VBEID:{1} Name:{2}({{{3}/{4}}}/{5}/{6})", tmpnum++, characterdata.Character.VBEID,
                        characterdata.Character.Name, characterdata.CurrentHP, characterdata.Character.HP, characterdata.Character.ATK, characterdata.Character.DEF);
                }
                while (string.IsNullOrEmpty(tmpstr))
                {
                    ConsoleWrite("Type new go on stage character VBEID (or type \"cancel\"):");
                    tmpstr = Console.ReadLine();
                }

                if (tmpstr.Equals("cancel", StringComparison.CurrentCultureIgnoreCase))
                {
                    changeCharacterActionModel = new()
                    {
                        NewCharacterVBEID = tmpstr,
                        IsChange = false
                    };
                    break;
                }
                else if (PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas.Any(x => x.Character.VBEID == tmpstr))
                {
                    if (tmpstr != PlayerDatas[(int)UserPlayerRelativeType.Self].CharacterDatas[0].Character.VBEID)
                    {
                        changeCharacterActionModel = new()
                        {
                            NewCharacterVBEID = tmpstr,
                            IsChange = true
                        };
                        break;
                    }
                    else
                    {
                        ConsoleWriteLine("Character on stage cannot be assigned.");
                    }
                }
                else
                {
                    ConsoleWriteLine("Character can not be found. Please try again.");
                }
            }

            LastChangeCharacterAction = changeCharacterActionModel;
            return changeCharacterActionModel;
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

        public void ShowSkillAnimate(ShowSkillAnimateModel data)
        {
            ConsoleWriteLine("Test_ShowActiveSkillAnimate");
            ConsoleWriteLine("Player: {0}, SkillID: {1}", data.Player.ToString(), data.SkillID);
            Thread.Sleep(1000);
        }

        public void BuffDataSet(BuffDataSetModel data)
        {
            ConsoleWriteLine("Test_BuffDataSet");
            ConsoleWriteLine("[{0}]{1}-{2}/{3}/{4}", data.Player.ToString(), data.CharacterVBEID, data.BuffData.Identifier, data.BuffData.Value, data.BuffData.Total);

            var characterData = PlayerDatas[(int)data.Player].CharacterDatas.Find(x => x.Character.VBEID == data.CharacterVBEID);

            if (characterData != null)
            {
                if (characterData.BuffDatas.Any(x => x.Identifier == data.BuffData.Identifier))
                {
                    var tmpdata = characterData.BuffDatas.First(x => x.Identifier == data.BuffData.Identifier);

                    tmpdata.Value = data.BuffData.Value;
                    tmpdata.Total = data.BuffData.Total;
                }
                else
                {
                    characterData.BuffDatas.Add(new()
                    {
                        Identifier = data.BuffData.Identifier,
                        Value = data.BuffData.Value,
                        Total = data.BuffData.Total
                    });
                }
            }
        }
        public void BuffDataUpdate(BuffDataUpdateModel data)
        {
            ConsoleWriteLine("Test_BuffDataUpdate");

            foreach (var player in System.Enum.GetValues<UserPlayerRelativeType>())
            {
                foreach (var characterData in PlayerDatas[(int)player].CharacterDatas)
                {
                    characterData.BuffDatas.Clear();
                    if (data.Datas[player].TryGetValue(characterData.Character.VBEID, out List<BuffDataBaseModel>? value))
                    {
                        characterData.BuffDatas.AddRange(value.Select(x => new BuffData
                        {
                            Identifier = x.Identifier,
                            Value = x.Value,
                            Total = x.Total,
                        }));
                    }
                }
            }
        }
        public void BuffDataRemove(BuffDataRemoveModel data)
        {
            ConsoleWriteLine("Test_BuffDataRemove");
            ConsoleWriteLine("[{0}]{1}-{2}", data.Player.ToString(), data.CharacterVBEID, data.BuffIdentifier);

            var characterData = PlayerDatas[(int)data.Player].CharacterDatas.Find(x => x.Character.VBEID == data.CharacterVBEID);

            if (characterData != null)
            {
                var tmpbuffData = characterData.BuffDatas.Find(x => x.Identifier == data.BuffIdentifier);
                if (tmpbuffData != null)
                {
                    characterData.BuffDatas.Remove(tmpbuffData);
                }
            }
        }

        public void ShowDice(ShowDiceModel data)
        {
            ConsoleWriteLine("Test_ShowDice");

            ConsoleWriteLine("SelfDiceTotal={0}", data.DiceTotal[(int)UserPlayerRelativeType.Self].ToString());
            ConsoleWriteLine("OpponentDiceTotal={0}", data.DiceTotal[(int)UserPlayerRelativeType.Opponent].ToString());
            ConsoleWriteLine("SelfDiceType={0}", data.DiceType[(int)UserPlayerRelativeType.Self].ToString());
            ConsoleWriteLine("OpponentDiceType={0}", data.DiceType[(int)UserPlayerRelativeType.Opponent].ToString());
            ConsoleWriteLine("SelfDiceTrue={0}", data.DiceTrue[(int)UserPlayerRelativeType.Self].ToString());
            ConsoleWriteLine("OpponentDiceTrue={0}", data.DiceTrue[(int)UserPlayerRelativeType.Opponent].ToString());
        }
    }
}
