using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        protected class UserActionProxy(BattleSystem battleSystem) : IUserAction
        {
            private PlayerData[] playerDatas = battleSystem.PlayerDatas;

            public bool BarMoveChange(UserPlayerType player)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect && battleSystem.PlayerVersusMode == PlayerVersusModeType.ThreeOnThree)
                {
                    playerDatas[(int)player].MoveBarSelect = MoveBarSelectType.Change;
                    return true;
                }
                return false;
            }

            public bool BarMoveLeft(UserPlayerType player)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect)
                {
                    playerDatas[(int)player].MoveBarSelect = MoveBarSelectType.Left;
                    return true;
                }
                return false;
            }

            public bool BarMoveRight(UserPlayerType player)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect)
                {
                    playerDatas[(int)player].MoveBarSelect = MoveBarSelectType.Right;
                    return true;
                }
                return false;
            }

            public bool BarMoveStay(UserPlayerType player)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect)
                {
                    playerDatas[(int)player].MoveBarSelect = MoveBarSelectType.Stay;
                    return true;
                }
                return false;
            }

            public UserActionCardClickType CardClick(UserPlayerType player, int cardNumber, PhaseType phaseType)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect)
                {
                    try
                    {
                        if (battleSystem.CardDeckIndex[cardNumber] == GetCardDeckType(player, ActionCardLocation.Hold) ||
                            battleSystem.CardDeckIndex[cardNumber] == GetCardDeckType(player, ActionCardLocation.Play))
                        {
                            var tmpcard = battleSystem.CardDecks[battleSystem.CardDeckIndex[cardNumber]][cardNumber];
                            ActionCardLocation origlocation = tmpcard.Location;
                            ActionCardLocation destlocation = tmpcard.Location == ActionCardLocation.Hold ? ActionCardLocation.Play : ActionCardLocation.Hold;
                            UserActionCardClickType clickType = origlocation == ActionCardLocation.Hold ? UserActionCardClickType.OUT : UserActionCardClickType.IN;

                            if (tmpcard.Location != ActionCardLocation.Hold && tmpcard.Location != ActionCardLocation.Play)
                            {
                                throw new Exception("Card Location Wrong.");
                            }

                            tmpcard.Location = destlocation;

                            battleSystem.DeckCardMove(tmpcard, GetCardDeckType(player, origlocation), GetCardDeckType(player, destlocation));

                            int tmpStageNum = phaseType switch
                            {
                                PhaseType.Move => 44,
                                PhaseType.Attack => 42,
                                PhaseType.Defense => 43,
                                _ => throw new NotImplementedException(),
                            };
                            battleSystem.SkillAdapter.StageStart(tmpStageNum, player, false, false);

                            if (phaseType == PhaseType.Attack || phaseType == PhaseType.Defense)
                            {
                                battleSystem.UpdatePlayerDiceTotalNumber(player, phaseType);
                            }

                            return clickType;
                        }
                        else
                        {
                            return UserActionCardClickType.None;
                        }
                    }
                    catch (Exception)
                    {
                        throw; //Debug
                    }
                }
                return UserActionCardClickType.None;
            }

            public UserActionCardReverseLocation CardReverse(UserPlayerType player, int cardNumber, PhaseType phaseType)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect)
                {
                    try
                    {
                        if (battleSystem.CardDeckIndex[cardNumber] == GetCardDeckType(player, ActionCardLocation.Hold) ||
                            battleSystem.CardDeckIndex[cardNumber] == GetCardDeckType(player, ActionCardLocation.Play))
                        {
                            var tmpcard = battleSystem.CardDecks[battleSystem.CardDeckIndex[cardNumber]][cardNumber];

                            if (tmpcard.Location != ActionCardLocation.Hold && tmpcard.Location != ActionCardLocation.Play)
                            {
                                throw new Exception("Card Location Wrong.");
                            }

                            tmpcard.Reverse();

                            if (tmpcard.Location == ActionCardLocation.Play)
                            {
                                int tmpStageNum = phaseType switch
                                {
                                    PhaseType.Move => 44,
                                    PhaseType.Attack => 42,
                                    PhaseType.Defense => 43,
                                    _ => throw new NotImplementedException(),
                                };
                                battleSystem.SkillAdapter.StageStart(tmpStageNum, player, false, false);
                            }

                            if (phaseType == PhaseType.Attack || phaseType == PhaseType.Defense)
                            {
                                battleSystem.UpdatePlayerDiceTotalNumber(player, phaseType);
                            }

                            return tmpcard.Location == ActionCardLocation.Hold ? UserActionCardReverseLocation.Hold : UserActionCardReverseLocation.Play;
                        }
                        else
                        {
                            return UserActionCardReverseLocation.None;
                        }

                    }
                    catch (Exception)
                    {
                        throw; //Debug
                    }
                }
                return UserActionCardReverseLocation.None;
            }

            public bool ChangeCharacter(UserPlayerType player, string NewCharacterVBEID)
            {
                var query = playerDatas[(int)player].CharacterDatas.Where(x => x.CurrentHP > 0 && x.Character.VBEID == NewCharacterVBEID);

                if (query.Any())
                {
                    var characterData = query.First();
                    playerDatas[(int)player].CharacterDatas.Remove(characterData);
                    playerDatas[(int)player].CharacterDatas.Insert(0, characterData);
                    return true;
                }

                return false;
            }

            public (bool, string) ChangeCharacterRandom(UserPlayerType player)
            {
                var query = playerDatas[(int)player].CharacterDatas.Where(x => x != playerDatas[(int)player].CurrentCharacter && x.CurrentHP > 0);

                if (query.Any())
                {
                    Random Rnd = new(DateTime.Now.Millisecond);
                    var tmpRndNum = Rnd.Next(query.Count());

                    var characterData = query.ElementAt(tmpRndNum);
                    playerDatas[(int)player].CharacterDatas.Remove(characterData);
                    playerDatas[(int)player].CharacterDatas.Insert(0, characterData);
                    return (true, characterData.Character.VBEID);
                }

                return (false, "");
            }

            public int GetPlayerDiceTotalNumber(UserPlayerType player)
            {
                return playerDatas[(int)player].DiceTotal;
            }

            public bool IsMustBeChangeCharacter(UserPlayerType player)
            {
                return playerDatas[(int)player].CurrentCharacter.CurrentHP <= 0;
            }

            public bool OKButtonClick(UserPlayerType player)
            {
                playerDatas[(int)player].IsOKButtonSelect = true;
                return true;
            }
        }
    }
}
