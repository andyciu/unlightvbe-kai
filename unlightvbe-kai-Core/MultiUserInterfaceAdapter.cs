using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.UserInterface;

namespace unlightvbe_kai_core
{
    public class MultiUserInterfaceAdapter
    {
        protected IUserInterface[] UserInterfaces { get; set; } = new IUserInterface[2];
        protected IUserAction UserActionProxy { get; set; }
        private readonly object dataLock = new();

        public MultiUserInterfaceAdapter(IUserInterface userInterface_P1, IUserInterface userInterface_P2, IUserAction userActionProxy)
        {
            UserInterfaces[(int)UserPlayerType.Player1] = userInterface_P1;
            UserInterfaces[(int)UserPlayerType.Player2] = userInterface_P2;
            UserActionProxy = userActionProxy;
        }

        public void ShowStartScreen(ShowStartScreenModel data)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].ShowStartScreen(new()
                {
                    PlayerSelfId = data.PlayerSelfId,
                    PlayerOpponentId = data.PlayerOpponentId,
                    PlayerSelf_CharacterVBEID = data.PlayerSelf_CharacterVBEID,
                    PlayerOpponent_CharacterVBEID = [data.PlayerOpponent_CharacterVBEID[0]]
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].ShowStartScreen(new()
                {
                    PlayerSelfId = data.PlayerOpponentId,
                    PlayerOpponentId = data.PlayerSelfId,
                    PlayerSelf_CharacterVBEID = data.PlayerOpponent_CharacterVBEID,
                    PlayerOpponent_CharacterVBEID = [data.PlayerSelf_CharacterVBEID[0]]
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
                            Owner = ActionCardRelativeOwner.Self,
                            Number = card.Number,
                            Identifier = card.Identifier,
                            IsReverse = card.IsReverse,
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
                            Owner = ActionCardRelativeOwner.Self,
                            Number = card.Number,
                            Identifier = card.Identifier,
                            IsReverse = card.IsReverse,
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
                    SelfCards =
                    [
                        new CardModel
                        {
                            UpperType = card_p1.UpperType,
                            UpperNum = card_p1.UpperNum,
                            LowerType = card_p1.LowerType,
                            LowerNum = card_p1.LowerNum,
                            Location = card_p1.Location,
                            Owner = ActionCardRelativeOwner.Self,
                            Number = card_p1.Number,
                            Identifier = card_p1.Identifier,
                            IsReverse = card_p1.IsReverse
                        }
                    ],
                    OpponentCardCount = 1,
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].DrawEventCard(new()
                {
                    SelfCards =
                    [
                        new CardModel
                        {
                            UpperType = card_p2.UpperType,
                            UpperNum = card_p2.UpperNum,
                            LowerType = card_p2.LowerType,
                            LowerNum = card_p2.LowerNum,
                            Location = card_p2.Location,
                            Owner = ActionCardRelativeOwner.Self,
                            Number = card_p2.Number,
                            Identifier = card_p2.Identifier,
                            IsReverse = card_p2.IsReverse
                        }
                    ],
                    OpponentCardCount = 1,
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void UpdateData_All(UpdateDataModel data)
        {
            Task task1 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player1].UpdateData(data); });
            Task task2 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player2].UpdateData(data); });
            Task.WhenAll(task1, task2).Wait();
        }

        public void UpdateData_RelativeMode(UpdateDataType type, UserPlayerType selfPlayer, string? message)
        {
            UserPlayerType OpponentPlayer = selfPlayer.GetOppenentPlayer();

            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)selfPlayer].UpdateData(new()
                {
                    Type = type,
                    Value = (int)UserPlayerRelativeType.Self,
                    Message = message
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)OpponentPlayer].UpdateData(new()
                {
                    Type = type,
                    Value = (int)UserPlayerRelativeType.Opponent,
                    Message = message
                });
            });

            Task.WhenAll(task1, task2).Wait();
        }

        public void UpdateDataMulti(UpdateDataMultiType type, UserPlayerType selfPlayer, int selfNumber, int oppenentNumber)
        {
            UserPlayerType OpponentPlayer = selfPlayer.GetOppenentPlayer();

            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].UpdateDataMulti(new()
                {
                    Type = type,
                    Self = selfNumber,
                    Opponent = oppenentNumber
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].UpdateDataMulti(new()
                {
                    Type = type,
                    Self = oppenentNumber,
                    Opponent = selfNumber
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void UpdateDataRelative(UpdateDataRelativeType type, UserPlayerType selfPlayer, int value, string? message)
        {
            UserPlayerType OpponentPlayer = selfPlayer.GetOppenentPlayer();

            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)selfPlayer].UpdateDataRelative(new()
                {
                    Type = type,
                    Player = UserPlayerRelativeType.Self,
                    Value = value,
                    Message = message
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)OpponentPlayer].UpdateDataRelative(new()
                {
                    Type = type,
                    Player = UserPlayerRelativeType.Opponent,
                    Value = value,
                    Message = message
                });
            });

            Task.WhenAll(task1, task2).Wait();
        }

        public void MovePhaseReadAction()
        {
            Task task1 = Task.Run(() => { ReadActionTask(UserPlayerType.Player1, PhaseType.Move); });
            Task task2 = Task.Run(() => { ReadActionTask(UserPlayerType.Player2, PhaseType.Move); });
            Task.WhenAll(task1, task2).Wait();
        }

        public void PhaseStart(PhaseStartModel data)
        {
            Task task1 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player1].PhaseStart(data); });
            Task task2 = Task.Run(() => { UserInterfaces[(int)UserPlayerType.Player2].PhaseStart(data); });
            Task.WhenAll(task1, task2).Wait();
        }

        public void PhaseStartAttackWithDefense(UserPlayerType attackPlayer)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].PhaseStart(new()
                {
                    Type = attackPlayer == UserPlayerType.Player1 ? PhaseType.Attack : PhaseType.Defense,
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].PhaseStart(new()
                {
                    Type = attackPlayer == UserPlayerType.Player2 ? PhaseType.Attack : PhaseType.Defense
                });
            });

            Task.WhenAll(task1, task2).Wait();
        }

        public void AttackWithDefensePhaseReadAction(UserPlayerType player, PhaseType phaseType)
        {
            Task.Run(() => { ReadActionTask(player, phaseType); }).Wait();
        }

        public void UpdateDiceTrueNumber(int p1_Total, int p2_Total)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].UpdateDataMulti(new()
                {
                    Type = UpdateDataMultiType.DiceTrue,
                    Self = p1_Total,
                    Opponent = p2_Total
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].UpdateDataMulti(new()
                {
                    Type = UpdateDataMultiType.DiceTrue,
                    Self = p2_Total,
                    Opponent = p1_Total
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void UpdateDiceTotalNumberRelative(UserPlayerType self_player, UserPlayerType opponent_player, int self_Total, int opponent_Total)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)self_player].UpdateDataMulti(new()
                {
                    Type = UpdateDataMultiType.DiceTotal,
                    Self = self_Total,
                    Opponent = opponent_Total
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)opponent_player].UpdateDataMulti(new()
                {
                    Type = UpdateDataMultiType.DiceTotal,
                    Self = opponent_Total,
                    Opponent = self_Total
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        private void ReadActionTask(UserPlayerType player, PhaseType phaseType)
        {
            UserInterfaces[(int)player].UpdateData(new()
            {
                Type = UpdateDataType.OKButtonOpen,
            });

            while (true)
            {
                var tmpAction = UserInterfaces[(int)player].ReadAction();

                var method = UserActionProxy.GetType().GetMethod(tmpAction.Type.ToString());
                object? result = null;
                if (method != null)
                {
                    lock (dataLock)
                    {
                        if (tmpAction.Type == UserActionType.CardClick || tmpAction.Type == UserActionType.CardReverse)
                        {
                            result = method.Invoke(UserActionProxy, [player, tmpAction.Value, phaseType]);
                        }
                        else if (phaseType == PhaseType.Move || tmpAction.Type == UserActionType.OKButtonClick)
                        {
                            result = method.Invoke(UserActionProxy, [player]);
                        }
                    }
                }
                if (result != null)
                {
                    UserPlayerType OpponentPlayer = player.GetOppenentPlayer();

                    if (tmpAction.Type == UserActionType.CardClick)
                    {
                        UserActionCardClickType clickType = (UserActionCardClickType)result;

                        if (clickType != UserActionCardClickType.None)
                        {
                            Task task1 = Task.Run(() =>
                            {
                                UserInterfaces[(int)player].ReadActionReceive(new()
                                {
                                    Type = clickType == UserActionCardClickType.OUT ? ReadActionReceiveType.PlayingCard : ReadActionReceiveType.HoldingCard,
                                    IsSuccess = true,
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

                            if (phaseType == PhaseType.Attack || phaseType == PhaseType.Defense)
                            {
                                UserInterfaces[(int)player].UpdateDataMulti(new()
                                {
                                    Type = UpdateDataMultiType.DiceTotal,
                                    Self = UserActionProxy.GetPlayerDiceTotalNumber(player),
                                    Opponent = UserActionProxy.GetPlayerDiceTotalNumber(OpponentPlayer)
                                });
                            }
                        }
                        else
                        {
                            UserInterfaces[(int)player].ReadActionReceive(new()
                            {
                                Type = ReadActionReceiveType.None,
                                IsSuccess = false,
                            });
                        }
                    }
                    else if (tmpAction.Type == UserActionType.CardReverse)
                    {
                        UserActionCardReverseLocation location = (UserActionCardReverseLocation)result;

                        UserInterfaces[(int)player].ReadActionReceive(new()
                        {
                            Type = location == UserActionCardReverseLocation.Hold ? ReadActionReceiveType.HoldingCardReverse :
                                    location == UserActionCardReverseLocation.Play ? ReadActionReceiveType.PlayingCardReverse :
                                    ReadActionReceiveType.None,
                            IsSuccess = location != UserActionCardReverseLocation.None
                        });

                        if (location != UserActionCardReverseLocation.None)
                        {
                            if (phaseType == PhaseType.Attack || phaseType == PhaseType.Defense)
                            {
                                UserInterfaces[(int)player].UpdateDataMulti(new()
                                {
                                    Type = UpdateDataMultiType.DiceTotal,
                                    Self = UserActionProxy.GetPlayerDiceTotalNumber(player),
                                    Opponent = UserActionProxy.GetPlayerDiceTotalNumber(OpponentPlayer)
                                });
                            }
                        }
                    }
                    else
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
                            },
                            IsSuccess = (bool)result
                        });
                    }
                }

                if (tmpAction.Type == UserActionType.OKButtonClick)
                {
                    break;
                }
            }
        }

        public void OpenOppenentPlayingCard(List<Card> cards_p1, List<Card> cards_p2)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].OpenOppenentPlayingCard(new()
                {
                    Cards = cards_p2.Select(card =>
                    {
                        return new CardModel
                        {
                            UpperType = card.UpperType,
                            UpperNum = card.UpperNum,
                            LowerType = card.LowerType,
                            LowerNum = card.LowerNum,
                            Location = card.Location,
                            Owner = ActionCardRelativeOwner.Opponent,
                            Number = card.Number,
                            Identifier = card.Identifier,
                            IsReverse = card.IsReverse,
                        };
                    }).ToList(),
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].OpenOppenentPlayingCard(new()
                {
                    Cards = cards_p1.Select(card =>
                    {
                        return new CardModel
                        {
                            UpperType = card.UpperType,
                            UpperNum = card.UpperNum,
                            LowerType = card.LowerType,
                            LowerNum = card.LowerNum,
                            Location = card.Location,
                            Owner = ActionCardRelativeOwner.Opponent,
                            Number = card.Number,
                            Identifier = card.Identifier,
                            IsReverse = card.IsReverse,
                        };
                    }).ToList(),
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void OpenOppenentPlayingCard(UserPlayerType selfPlayer, List<Card> opponentCards)
        {
            Task.Run(() =>
            {
                UserInterfaces[(int)selfPlayer].OpenOppenentPlayingCard(new()
                {
                    Cards = opponentCards.Select(card =>
                    {
                        return new CardModel
                        {
                            UpperType = card.UpperType,
                            UpperNum = card.UpperNum,
                            LowerType = card.LowerType,
                            LowerNum = card.LowerNum,
                            Location = card.Location,
                            Owner = ActionCardRelativeOwner.Opponent,
                            Number = card.Number,
                            Identifier = card.Identifier,
                            IsReverse = card.IsReverse,
                        };
                    }).ToList(),
                });
            }).Wait();
        }

        public void ChangeCharacterAction(UserPlayerType player)
        {
            Task.Run(() => { ChangeCharacterActionTask(player); }).Wait();
        }

        private void ChangeCharacterActionTask(UserPlayerType player)
        {
            UserPlayerType OpponentPlayer = player.GetOppenentPlayer();
            string playerNewCharacterVBEID;

            UserInterfaces[(int)OpponentPlayer].UpdateData(new()
            {
                Type = UpdateDataType.OpponentCharacterChangeBegin
            });

            while (true)
            {
                var actionModel = UserInterfaces[(int)player].ChangeCharacterAction();
                UserInterfaces[(int)player].ShowBattleMessage(actionModel.NewCharacterVBEID);

                var result = UserActionProxy.ChangeCharacter(player, actionModel.NewCharacterVBEID);
                UserInterfaces[(int)player].ReadActionReceive(new()
                {
                    Type = ReadActionReceiveType.ChangeCharacter,
                    IsSuccess = result
                });

                if (result)
                {
                    playerNewCharacterVBEID = actionModel.NewCharacterVBEID;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(playerNewCharacterVBEID))
            {
                UserInterfaces[(int)OpponentPlayer].UpdateData(new()
                {
                    Type = UpdateDataType.OpponentCharacterChangeAction,
                    Message = playerNewCharacterVBEID
                });
            }
        }

        public void ShowJudgment(ShowJudgmentType type_p1, ShowJudgmentType type_p2)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].ShowJudgment(new()
                {
                    Type = type_p1
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].ShowJudgment(new()
                {
                    Type = type_p2
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void ActiveSkillLineLight(UserPlayerType player, int skillIndex, bool isLighting)
        {
            Task.Run(() =>
            {
                UserInterfaces[(int)player].UpdateData(new()
                {
                    Type = UpdateDataType.ActiveSkillLineLight,
                    Value = skillIndex,
                    Message = isLighting.ToString()
                });
            }).Wait();
        }

        public void ShowSkillAnimate(UserPlayerType selfPlayer, string skillVBEID)
        {
            UserPlayerType opponentPlayer = selfPlayer.GetOppenentPlayer();

            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)selfPlayer].ShowSkillAnimate(new()
                {
                    Player = UserPlayerRelativeType.Self,
                    SkillID = skillVBEID
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)opponentPlayer].ShowSkillAnimate(new()
                {
                    Player = UserPlayerRelativeType.Opponent,
                    SkillID = skillVBEID
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void BuffDataSet(UserPlayerType selfPlayer, string characterVBEID, BuffDataBaseModel buffData)
        {
            UserPlayerType opponentPlayer = selfPlayer.GetOppenentPlayer();

            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)selfPlayer].BuffDataSet(new()
                {
                    Player = UserPlayerRelativeType.Self,
                    CharacterVBEID = characterVBEID,
                    BuffData = buffData
                });
            });

            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)opponentPlayer].BuffDataSet(new()
                {
                    Player = UserPlayerRelativeType.Opponent,
                    CharacterVBEID = characterVBEID,
                    BuffData = buffData
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void BuffDataUpdate(Dictionary<string, List<BuffDataBaseModel>> data_p1, Dictionary<string, List<BuffDataBaseModel>> data_p2)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player1].BuffDataUpdate(new()
                {
                    Datas = new()
                    {
                        { UserPlayerRelativeType.Self, data_p1 },
                        { UserPlayerRelativeType.Opponent, data_p2 }
                    }
                });
            });

            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)UserPlayerType.Player2].BuffDataUpdate(new()
                {
                    Datas = new()
                    {
                        { UserPlayerRelativeType.Self, data_p2 },
                        { UserPlayerRelativeType.Opponent, data_p1 }
                    }
                });
            });

            Task.WhenAll(task1, task2).Wait();
        }

        public void BuffDataRemove(UserPlayerType selfPlayer, string characterVBEID, string buffIdentifier)
        {
            UserPlayerType opponentPlayer = selfPlayer.GetOppenentPlayer();

            Task task1 = Task.Run(() =>
            {
                UserInterfaces[(int)selfPlayer].BuffDataRemove(new()
                {
                    Player = UserPlayerRelativeType.Self,
                    CharacterVBEID = characterVBEID,
                    BuffIdentifier = buffIdentifier
                });
            });

            Task task2 = Task.Run(() =>
            {
                UserInterfaces[(int)opponentPlayer].BuffDataRemove(new()
                {
                    Player = UserPlayerRelativeType.Opponent,
                    CharacterVBEID = characterVBEID,
                    BuffIdentifier = buffIdentifier
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }
    }
}
