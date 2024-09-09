using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 牌堆集合洗牌
        /// </summary>
        /// <param name="origdeck">原始集合</param>
        /// <returns></returns>
        private Dictionary<int, Card> ShuffleDeck(Dictionary<int, Card> origdeck)
        {
            var tmpdeck = new Dictionary<int, Card>(origdeck);
            var newdeck = new Dictionary<int, Card>();

            while (tmpdeck.Count > 0)
            {
                var tmpnum = Rnd.Next(tmpdeck.Count);
                newdeck.Add(tmpdeck.ElementAt(tmpnum).Key, tmpdeck.ElementAt(tmpnum).Value);
                tmpdeck.Remove(tmpdeck.ElementAt(tmpnum).Key);
            }

            return newdeck;
        }

        /// <summary>
        /// 墓地牌洗牌前重新倒回牌堆
        /// </summary>
        private void GraveyardDeckReUse()
        {
            foreach (var card in CardDecks[CardDeckType.Graveyard])
            {
                card.Value.Location = ActionCardLocation.Deck;
                card.Value.Owner = ActionCardOwner.System;

                DeckCardMove(card.Value, CardDeckType.Graveyard, CardDeckType.Deck);
            }
        }

        /// <summary>
        /// 卡牌於牌堆集合間移動
        /// </summary>
        /// <param name="card">卡牌</param>
        /// <param name="origType">目前所在集合</param>
        /// <param name="destType">目標集合</param>
        private void DeckCardMove(Card card, CardDeckType origType, CardDeckType destType)
        {
            CardDecks[destType].Add(card.Number, card);
            CardDecks[origType].Remove(card.Number);

            CardDeckIndex[card.Number] = destType;
        }

        /// <summary>
        /// 取得卡牌編號
        /// </summary>
        /// <param name="cardDeckType">目標集合</param>
        /// <returns></returns>
        private int GetCardIndex(CardDeckType cardDeckType)
        {
            int newNum = CardDeckIndex.Count + 1;
            CardDeckIndex.Add(newNum, cardDeckType);

            return newNum;
        }

        /// <summary>
        /// (初始階段)匯入場地行動卡資訊
        /// </summary>
        private void ImportActionCardToDeck()
        {
            var cardlist = SampleData.GetCardList_Deck();

            foreach (var card in cardlist)
            {
                int tmpnum = GetCardIndex(CardDeckType.Deck);
                card.Number = tmpnum;
                card.Owner = ActionCardOwner.System;
                card.Location = ActionCardLocation.Deck;

                CardDecks[CardDeckType.Deck].Add(tmpnum, card);
            }
        }

        /// <summary>
        /// (初始階段)匯入玩家事件卡資訊
        /// </summary>
        private void ImportEventCardToDeck()
        {
            for (int i = 0; i < PlayerDatas.Length; i++)
            {
                var player = PlayerDatas[i];
                foreach (var sub in player.Player.Deck.Deck_Subs)
                {
                    foreach (var card in sub.eventCards)
                    {
                        CardDeckType cardDeckType = GetCardDeckType((UserPlayerType)i, ActionCardLocation.Deck);

                        int tmpnum = GetCardIndex(cardDeckType);
                        card.Number = tmpnum;
                        card.Owner = ActionCardOwner.System;
                        card.Location = ActionCardLocation.Deck;

                        CardDecks[cardDeckType].Add(tmpnum, card);
                    }
                }
            }
        }

        /// <summary>
        /// 玩家事件卡預設補充
        /// </summary>
        private void EvnetCardComplement()
        {
            for (int n = 0; n < PlayerDatas.Length; n++)
            {
                CardDeckType cardDeckType = GetCardDeckType((UserPlayerType)n, ActionCardLocation.Deck);
                if (CardDecks[cardDeckType].Count < TurnMaxNum)
                {
                    for (int i = CardDecks[cardDeckType].Count; i < TurnMaxNum; i++)
                    {
                        int tmpnum = GetCardIndex(cardDeckType);
                        Card tmpcard = GetDefaultEventCard(Rnd.Next(3));
                        tmpcard.Number = tmpnum;
                        tmpcard.Owner = ActionCardOwner.System;
                        tmpcard.Location = ActionCardLocation.Deck;
                        CardDecks[cardDeckType].Add(tmpnum, tmpcard);
                    }
                }
            }
        }

        private EventCard GetDefaultEventCard(int typenum)
        {
            return typenum switch
            {
                0 => new EventCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = 1,
                    LowerType = ActionCardType.ATK_Sword,
                    LowerNum = 1,
                },
                1 => new EventCard
                {
                    UpperType = ActionCardType.ATK_Gun,
                    UpperNum = 1,
                    LowerType = ActionCardType.ATK_Gun,
                    LowerNum = 1,
                },
                2 => new EventCard
                {
                    UpperType = ActionCardType.DEF,
                    UpperNum = 1,
                    LowerType = ActionCardType.DEF,
                    LowerNum = 1,
                },
                _ => new EventCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = 1,
                    LowerType = ActionCardType.ATK_Sword,
                    LowerNum = 1,
                },
            };
        }

        private CardDeckType GetCardDeckType(ActionCardOwner owner, ActionCardLocation location)
        {
            return location switch
            {
                ActionCardLocation.Hold => owner switch
                {
                    ActionCardOwner.Player1 => CardDeckType.Hold_P1,
                    ActionCardOwner.Player2 => CardDeckType.Hold_P2,
                    _ => throw new NotImplementedException(),
                },
                ActionCardLocation.Play => owner switch
                {
                    ActionCardOwner.Player1 => CardDeckType.Play_P1,
                    ActionCardOwner.Player2 => CardDeckType.Play_P2,
                    _ => throw new NotImplementedException(),
                },
                ActionCardLocation.Graveyard => CardDeckType.Graveyard,
                ActionCardLocation.Fold => CardDeckType.Fold,
                ActionCardLocation.Deck => owner switch
                {
                    ActionCardOwner.Player1 => CardDeckType.Event_P1,
                    ActionCardOwner.Player2 => CardDeckType.Event_P2,
                    ActionCardOwner.System => CardDeckType.Deck,
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };
        }

        private CardDeckType GetCardDeckType(UserPlayerType player, ActionCardLocation location)
        {
            return location switch
            {
                ActionCardLocation.Hold => player switch
                {
                    UserPlayerType.Player1 => CardDeckType.Hold_P1,
                    UserPlayerType.Player2 => CardDeckType.Hold_P2,
                    _ => throw new NotImplementedException(),
                },
                ActionCardLocation.Play => player switch
                {
                    UserPlayerType.Player1 => CardDeckType.Play_P1,
                    UserPlayerType.Player2 => CardDeckType.Play_P2,
                    _ => throw new NotImplementedException(),
                },
                ActionCardLocation.Deck => player switch
                {
                    UserPlayerType.Player1 => CardDeckType.Event_P1,
                    UserPlayerType.Player2 => CardDeckType.Event_P2,
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// 取得玩家卡牌之類型總數
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="location">卡牌位置</param>
        /// <returns>各類型對應總數集合</returns>
        private Dictionary<ActionCardType, int> GetCardTotalNumber(UserPlayerType player, ActionCardLocation location)
        {
            var result = new Dictionary<ActionCardType, int>();

            foreach (var card in CardDecks[GetCardDeckType(player, location)])
            {
                if (result.ContainsKey(card.Value.UpperType))
                {
                    result[card.Value.UpperType] += card.Value.UpperNum;
                }
                else
                {
                    result.Add(card.Value.UpperType, card.Value.UpperNum);
                }
            }

            return result;
        }

        /// <summary>
        /// 取得玩家計算後總骰數
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="phase">判斷階段</param>
        /// <param name="distance">判斷距離</param>
        /// <returns></returns>
        private int GetPlayerDiceTotalNumber(UserPlayerType player, PhaseType phase, PlayerDistanceType distance)
        {
            var result = 0;
            var cardtotal = GetCardTotalNumber(player, ActionCardLocation.Play);
            int tmptotalnum;

            switch (phase)
            {
                case PhaseType.Attack:
                    ActionCardType ATKcardType = distance == PlayerDistanceType.Close ? ActionCardType.ATK_Sword : ActionCardType.ATK_Gun;

                    if (cardtotal.TryGetValue(ATKcardType, out tmptotalnum))
                    {
                        result += tmptotalnum;
                        result += PlayerDatas[(int)player].CurrentCharacter.Character.ATK;
                    }

                    break;
                case PhaseType.Defense:
                    if (cardtotal.TryGetValue(ActionCardType.DEF, out tmptotalnum))
                    {
                        result += tmptotalnum;
                    }

                    result += PlayerDatas[(int)player].CurrentCharacter.Character.DEF;

                    break;
            }

            return result;
        }

        /// <summary>
        /// 更新玩家總骰數資料
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="phase">判斷階段</param>
        private void UpdatePlayerDiceTotalNumber(UserPlayerType player, PhaseType phase)
        {
            PlayerDatas[(int)player].DiceTotal = GetPlayerDiceTotalNumber(player, phase, PlayerDistance);

            UserPlayerType playerType_Opponent = player == UserPlayerType.Player1 ? UserPlayerType.Player2 : UserPlayerType.Player1;
            PhaseType phaseType_Opponent = phase == PhaseType.Attack ? PhaseType.Defense : PhaseType.Attack;

            PlayerDatas[(int)playerType_Opponent].DiceTotal = GetPlayerDiceTotalNumber(playerType_Opponent, phaseType_Opponent, PlayerDistance);
        }

        /// <summary>
        /// 場上牌收牌
        /// </summary>
        private void CollectPlayingCardToGraveyard()
        {
            int[] collectTotal = new int[2];
            int currentPlayerFlag = 0;

            foreach (var player in System.Enum.GetValues<UserPlayerType>())
            {
                var cardtype = GetCardDeckType(player, ActionCardLocation.Play);
                foreach (var card in CardDecks[cardtype])
                {
                    if (card.Value.GetType().Equals(typeof(EventCard)))
                    {
                        card.Value.Location = ActionCardLocation.Fold;
                        card.Value.Owner = ActionCardOwner.System;

                        DeckCardMove(card.Value, cardtype, CardDeckType.Fold);
                    }
                    else
                    {
                        card.Value.Location = ActionCardLocation.Graveyard;
                        card.Value.Owner = ActionCardOwner.System;

                        DeckCardMove(card.Value, cardtype, CardDeckType.Graveyard);
                    }
                    collectTotal[currentPlayerFlag]++;
                }
                currentPlayerFlag++;
            }

            MultiUIAdapter.UpdateDataMulti(UpdateDataMultiType.PlayedCardCollectCount, UserPlayerType.Player1, collectTotal[0], collectTotal[1]);
        }

        /// <summary>
        /// 擲骰動作執行
        /// </summary>
        /// <param name="diceTotal">總骰數</param>
        /// <returns>擲骰有效數</returns>
        private int DiceAction(int diceTotal)
        {
            int result = 0;

            for (int j = 0; j < diceTotal; j++)
            {
                result += Convert.ToInt32(Dice.Roll());
            }

            return result;
        }

        /// <summary>
        /// 雙方玩家角色存活確認
        /// </summary>
        /// <returns>是否通過存活確認</returns>
        private bool PlayerCharacterHPCheck()
        {
            foreach (var player in PlayerDatas)
            {
                if (player.CurrentCharacter.CurrentHP <= 0)
                {
                    bool isReplaceable = false;
                    foreach (var character in player.CharacterDatas)
                    {
                        if (character.CurrentHP > 0)
                        {
                            isReplaceable = true;
                            break;
                        }
                    }
                    if (isReplaceable)
                    {
                        MultiUIAdapter.ChangeCharacterAction(player.PlayerType);
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 角色傷害執行
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="characterVBEID">角色VBEID</param>
        /// <param name="damageNumber">傷害數</param>
        private void CharacterHPDamage(UserPlayerType player, string characterVBEID, int damageNumber)
        {
            var characterData = PlayerDatas[(int)player].GetCharacterData(characterVBEID);
            if (characterData == null) throw new Exception("characterVBEID null reference");

            int origHP = characterData.CurrentHP;

            characterData.CurrentHP -= damageNumber;
            if (characterData.CurrentHP < 0) characterData.CurrentHP = 0;

            MultiUIAdapter.UpdateDataRelative(UpdateDataRelativeType.CharacterHPDamage, player, origHP - characterData.CurrentHP, characterVBEID);
        }

        /// <summary>
        /// 角色回復執行
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="characterVBEID">角色VBEID</param>
        /// <param name="healNumber">回復數</param>
        private void CharacterHPHeal(UserPlayerType player, string characterVBEID, int healNumber)
        {
            var characterData = PlayerDatas[(int)player].GetCharacterData(characterVBEID);
            if (characterData == null) throw new Exception("characterVBEID null reference");

            if (characterData.CurrentHP < characterData.Character.HP)
            {
                int origHP = characterData.CurrentHP;

                characterData.CurrentHP += healNumber;
                if (characterData.CurrentHP > characterData.Character.HP) characterData.CurrentHP = characterData.Character.HP;

                MultiUIAdapter.UpdateDataRelative(UpdateDataRelativeType.CharacterHPHeal, player, characterData.CurrentHP - origHP, characterVBEID);
            }
        }
    }
}
