using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_console
{
    internal class ConsoleInterface : IUserInterfaceAsync
    {
        public string InstanceName { get; set; }
        private List<CardModel> HoldCards { get; set; } = new List<CardModel>();
        private List<CardModel> PlayCards { get; set; } = new List<CardModel>();
        private int OpponentHoldCardCount;
        private int OpponentPlayCardCount;
        private int TurnNum;
        /// <summary>
        /// 牌堆卡牌數
        /// </summary>
        private int DeckNum;
        private int[] PlayerIds = new int[2];
        private int[] PlayerDeckIndex = new int[2];
        private ReadActionModel? LastReadAction = null;
        private int[] DiceTotalNum = new int[2];
        private MoveBarSelectType MoveBarSelectType;
        private bool IsOKButtonClick;
        public ConsoleInterface(string instanceName)
        {
            InstanceName = instanceName;
        }

        internal virtual string ShowInstanceName()
        {
            return "[" + InstanceName + "]";
        }

        internal virtual void SetConsoleColor()
        {
            Console.ResetColor();
        }

        internal void ConsoleWriteLine(string message)
        {
            lock (Console.Out)
            {
                SetConsoleColor();
                Console.WriteLine(ShowInstanceName() + message);
            }
        }

        internal void ConsoleWriteLine(string message, params object?[]? arg)
        {
            lock (Console.Out)
            {
                SetConsoleColor();
                Console.WriteLine(ShowInstanceName() + message, arg);
            }
        }

        internal void ConsoleWrite(string message)
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
            ConsoleWriteLine("Player1_Id=" + data.Player1_Id);
            ConsoleWriteLine("Player1_DeckIndex=" + data.Player1_DeckIndex);

            Thread.Sleep(1000);

            ConsoleWriteLine("Player2_Id=" + data.Player2_Id);
            ConsoleWriteLine("Player2_DeckIndex=" + data.Player2_DeckIndex);
            PlayerIds[(int)UserPlayerType.Player1] = data.Player1_Id;
            PlayerIds[(int)UserPlayerType.Player2] = data.Player2_Id;
            PlayerDeckIndex[(int)UserPlayerType.Player1] = data.Player1_DeckIndex;
            PlayerDeckIndex[(int)UserPlayerType.Player2] = data.Player2_DeckIndex;
        }

        public void ShowBattleMessage(string message)
        {
            ConsoleWriteLine("Test_ShowBattleMessage");
            ConsoleWriteLine(message);
        }
        public virtual ReadActionModel ReadAction()
        {
            ReadActionModel? readActionModel = null;

            while (readActionModel == null)
            {
                string tmpstr = string.Empty;
                while (string.IsNullOrEmpty(tmpstr))
                {
                    Console.Write(ShowInstanceName() + "Action(\"Type Message\"): ");
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
                    case "SHOWCARD":
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
                    case "SHOWTURN":
                        ConsoleWriteLine("Turn=" + TurnNum);
                        break;
                    case "SHOWDECK":
                        ConsoleWriteLine("Deck=" + DeckNum);
                        ConsoleWriteLine("SelfHold=" + HoldCards.Count);
                        ConsoleWriteLine("SelfPlay=" + PlayCards.Count);
                        ConsoleWriteLine("OpponentHold=" + OpponentHoldCardCount);
                        ConsoleWriteLine("OpponentPlay=" + OpponentPlayCardCount);
                        break;
                    default:
                        ConsoleWriteLine("Action Unknown. Please try again.");
                        break;
                }
            }

            LastReadAction = readActionModel;
            return readActionModel;
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
            }

            OpponentHoldCardCount += data.OpponentCardCount;
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
            ConsoleWriteLine(data.Type.ToString() + ":" + data.Value);

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
                case UpdateDataType.SelfDiceTotalNumber:
                    DiceTotalNum[(int)UserPlayerType.Player1] = data.Value;
                    break;
                case UpdateDataType.OpponentDiceTotalNumber:
                    DiceTotalNum[(int)UserPlayerType.Player2] = data.Value;
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
            }
        }
    }
}
