using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        protected class UserActionProxy : IUserAction
        {
            private BattleSystem BattleSystem;
            private PlayerData[] playerDatas;
            public UserActionProxy(BattleSystem battleSystem)
            {
                BattleSystem = battleSystem;
                playerDatas = battleSystem.PlayerDatas;
            }

            public bool BarMoveChange(UserPlayerType player)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect)
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

            public UserActionCardClickType CardClick(UserPlayerType player, int cardNumber)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect)
                {
                    try
                    {
                        var tmpcard = BattleSystem.CardDecks[(int)BattleSystem.CardDeckIndex[cardNumber]][cardNumber];
                        ActionCardLocation origlocation = tmpcard.Location;
                        ActionCardLocation destlocation = tmpcard.Location == ActionCardLocation.Hold ? ActionCardLocation.Play : ActionCardLocation.Hold;
                        UserActionCardClickType clickType = origlocation == ActionCardLocation.Hold ? UserActionCardClickType.OUT : UserActionCardClickType.IN;

                        if (tmpcard.Location != ActionCardLocation.Hold && tmpcard.Location != ActionCardLocation.Play)
                        {
                            throw new Exception("Card Location Wrong.");
                        }

                        tmpcard.Location = destlocation;

                        BattleSystem.DeckCardMove(tmpcard, BattleSystem.GetCardDeckType(player, origlocation), BattleSystem.GetCardDeckType(player, destlocation));
                        return clickType;
                    }
                    catch (Exception)
                    {
                        throw; //Debug
                    }
                }
                return UserActionCardClickType.None;
            }

            public UserActionCardReverseLocation CardReverse(UserPlayerType player, int cardNumber)
            {
                if (!playerDatas[(int)player].IsOKButtonSelect)
                {
                    try
                    {
                        var tmpcard = BattleSystem.CardDecks[(int)BattleSystem.CardDeckIndex[cardNumber]][cardNumber];

                        if (tmpcard.Location != ActionCardLocation.Hold && tmpcard.Location != ActionCardLocation.Play)
                        {
                            throw new Exception("Card Location Wrong.");
                        }

                        tmpcard.Reverse();

                        return tmpcard.Location == ActionCardLocation.Hold ? UserActionCardReverseLocation.Hold : UserActionCardReverseLocation.Play;
                    }
                    catch (Exception)
                    {
                        throw; //Debug
                    }
                }
                return UserActionCardReverseLocation.None;
            }

            public bool OKButtonClick(UserPlayerType player)
            {
                playerDatas[(int)player].IsOKButtonSelect = true;
                return true;
            }
        }
    }
}
