using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_core
{
    public class MultiUserInterfaceAdapter
    {
        protected IUserInterfaceAsync[] UserInterfaces { get; set; } = new IUserInterfaceAsync[2];
        protected IUserAction UserActionProxy { get; set; }
        private readonly object dataLock = new object();

        public MultiUserInterfaceAdapter(IUserInterfaceAsync userInterface_P1, IUserInterfaceAsync userInterface_P2, IUserAction userActionProxy)
        {
            UserInterfaces[(int)UserPlayerType.Player1] = userInterface_P1;
            UserInterfaces[(int)UserPlayerType.Player2] = userInterface_P2;
            UserActionProxy = userActionProxy;
        }

        public void ShowStartScreen(ShowStartScreenModel data)
        {
            Task task1 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player1].ShowStartScreen(data); });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].ShowStartScreen(new()
                {
                    Player1_Id = data.Player2_Id,
                    Player2_Id = data.Player1_Id,
                    Player1_DeckIndex = data.Player2_DeckIndex,
                    Player2_DeckIndex = data.Player1_DeckIndex,
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void ShowBattleMessage(string message)
        {
            Task task1 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player1].ShowBattleMessage(message); });
            Task task2 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player2].ShowBattleMessage(message); });
            Task.WhenAll(task1, task2).Wait();
        }

        public void DrawActionCard(List<Card> cards_p1, List<Card> cards_p2)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].DrawActionCard(new()
                {
                    SelfCards = cards_p1.Select(card =>
                    {
                        return new CardModel
                        {
                            UpperType = card.UpperType,
                            UpperNum = card.UpperNum,
                            LowerType = card.LowerType,
                            LowerNum = card.LowerNum,
                            Location = card.Location,
                            Owner = card.Owner,
                            Number = card.Number,
                        };
                    }).ToList(),
                    OpponentCardCount = cards_p2.Count,
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].DrawActionCard(new()
                {
                    SelfCards = cards_p2.Select(card =>
                    {
                        return new CardModel
                        {
                            UpperType = card.UpperType,
                            UpperNum = card.UpperNum,
                            LowerType = card.LowerType,
                            LowerNum = card.LowerNum,
                            Location = card.Location,
                            Owner = card.Owner,
                            Number = card.Number,
                        };
                    }).ToList(),
                    OpponentCardCount = cards_p1.Count,
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void DrawEventCard(EventCard card_p1, EventCard card_p2)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].DrawEventCard(new()
                {
                    SelfCards = new List<CardModel>()
                    {
                        new CardModel
                        {
                            UpperType = card_p1.UpperType,
                            UpperNum = card_p1.UpperNum,
                            LowerType = card_p1.LowerType,
                            LowerNum = card_p1.LowerNum,
                            Location = card_p1.Location,
                            Owner = card_p1.Owner,
                            Number = card_p1.Number,
                        }
                    },
                    OpponentCardCount = 1,
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].DrawEventCard(new()
                {
                    SelfCards = new List<CardModel>()
                    {
                        new CardModel
                        {
                            UpperType = card_p2.UpperType,
                            UpperNum = card_p2.UpperNum,
                            LowerType = card_p2.LowerType,
                            LowerNum = card_p2.LowerNum,
                            Location = card_p2.Location,
                            Owner = card_p2.Owner,
                            Number = card_p2.Number,
                        }
                    },
                    OpponentCardCount = 1,
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void UpdateData(UpdateDataModel data)
        {
            Task task1 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player1].UpdateData(data); });
            Task task2 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player2].UpdateData(data); });
            Task.WhenAll(task1, task2).Wait();
        }

        public void MovePhaseReadAction()
        {
            Task task1 = Task.Run(() => { ReadActionTask(UserPlayerType.Player1); });
            Task task2 = Task.Run(() => { ReadActionTask(UserPlayerType.Player2); });
            Task.WhenAll(task1, task2).Wait();
        }

        private void ReadActionTask(UserPlayerType player)
        {
            while (true)
            {
                var tmpAction = UserInterfaces[(int)player].ReadAction();
                UserInterfaces[(int)player].ShowBattleMessage(tmpAction.Type.ToString() + " " + tmpAction.Message);

                var method = UserActionProxy.GetType().GetMethod(tmpAction.Type.ToString());
                object? result = null;
                if (method != null)
                {
                    lock (dataLock)
                    {
                        if (tmpAction.Type == UserActionType.CardClick || tmpAction.Type == UserActionType.CardReverse)
                        {
                            result = method.Invoke(UserActionProxy, new object[] { player, tmpAction.Value });
                        }
                        else
                        {
                            result = method.Invoke(UserActionProxy, new object[] { player });
                        }
                    }
                }
                if (result != null)
                {
                    if (tmpAction.Type == UserActionType.CardClick)
                    {
                        UserActionCardClickType clickType = (UserActionCardClickType)result;
                        if (clickType != UserActionCardClickType.None)
                        {
                            UserPlayerType OpponentPlayer = player == UserPlayerType.Player1 ? UserPlayerType.Player2 : UserPlayerType.Player1;

                            Task task1 = Task.Run(() =>
                            {
                                UserInterfaces[(int)player].ReadActionReceive(new()
                                {
                                    Type = clickType == UserActionCardClickType.OUT ? ReadActionReceiveType.PlayingCard : ReadActionReceiveType.HoldingCard,
                                });
                            });
                            Task task2 = Task.Run(() =>
                            {
                                UserInterfaces[(int)OpponentPlayer].UpdateData(new()
                                {
                                    Type = clickType == UserActionCardClickType.OUT ? UpdateDataType.OpponentPlayingCardCount : UpdateDataType.OpponentHoldingCardCount,
                                    Value = 1
                                });
                            });
                            Task.WhenAll(task1, task2).Wait();
                        }
                    }
                    else if (tmpAction.Type == UserActionType.CardReverse)
                    {
                        UserActionCardReverseLocation location = (UserActionCardReverseLocation)result;

                        if (location != UserActionCardReverseLocation.None)
                        {
                            UserInterfaces[(int)player].ReadActionReceive(new()
                            {
                                Type = location == UserActionCardReverseLocation.Hold ? ReadActionReceiveType.HoldingCardReverse : ReadActionReceiveType.PlayingCardReverse,
                            });
                        }
                    }
                    else
                    {
                        if ((bool)result)
                        {
                            UserInterfaces[(int)player].ReadActionReceive(new()
                            {
                                Type = tmpAction.Type switch
                                {
                                    UserActionType.BarMoveLeft => ReadActionReceiveType.BarMoveLeft,
                                    UserActionType.BarMoveRight => ReadActionReceiveType.BarMoveRight,
                                    UserActionType.BarMoveStay => ReadActionReceiveType.BarMoveStay,
                                    UserActionType.BarMoveChange => ReadActionReceiveType.BarMoveChange,
                                    UserActionType.OKButtonClick => ReadActionReceiveType.OKButtonClick,
                                    _ => throw new NotImplementedException()
                                }
                            });
                        }
                    }
                }

                if (tmpAction.Type == UserActionType.OKButtonClick)
                {
                    break;
                }
            }
        }
    }
}
