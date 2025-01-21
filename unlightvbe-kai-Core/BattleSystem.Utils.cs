using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.Skill;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Enum.UserInterface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.UserInterface;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 牌堆集合洗牌
        /// </summary>
        /// <param name="origdeck">原始集合</param>
        /// <returns></returns>
        private static Dictionary<int, Card> ShuffleDeck(Dictionary<int, Card> origdeck)
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
            while (CardDecks[CardDeckType.Graveyard].Count > 0)
            {
                var tmpCard = CardDecks[CardDeckType.Graveyard].First();
                tmpCard.Value.Location = ActionCardLocation.Deck;
                tmpCard.Value.Owner = ActionCardOwner.System;

                DeckCardMove(tmpCard.Value, CardDeckType.Graveyard, CardDeckType.Deck);
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
            for (int i = 0; i < InitialCardDeck.Count; i++)
            {
                var tmpCard = new ActionCard(InitialCardDeck[i]);
                int tmpnum = GetCardIndex(CardDeckType.Deck);
                tmpCard.Number = tmpnum;
                tmpCard.Owner = ActionCardOwner.System;
                tmpCard.Location = ActionCardLocation.Deck;

                CardDecks[CardDeckType.Deck].Add(tmpnum, tmpCard);
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
                    if (sub.EventCards == null) continue;
                    for (int j = 0; j < sub.EventCards.Count; j++)
                    {
                        var tmpCard = new EventCard(sub.EventCards[j]);
                        CardDeckType cardDeckType = GetCardDeckType((UserPlayerType)i, ActionCardLocation.Deck);

                        int tmpnum = GetCardIndex(cardDeckType);
                        tmpCard.Number = tmpnum;
                        tmpCard.Owner = ActionCardOwner.System;
                        tmpCard.Location = ActionCardLocation.Deck;

                        CardDecks[cardDeckType].Add(tmpnum, tmpCard);
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

        private static EventCard GetDefaultEventCard(int typenum)
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

        private static CardDeckType GetCardDeckType(ActionCardOwner owner, ActionCardLocation location)
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

        private static CardDeckType GetCardDeckType(UserPlayerType player, ActionCardLocation location)
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

            foreach (var type in System.Enum.GetValues<ActionCardType>())
            {
                result.TryAdd(type, 0);
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
            var personAbilityNum = 0;
            var cardtotal = GetCardTotalNumber(player, ActionCardLocation.Play);
            int tmptotalnum;

            switch (phase)
            {
                case PhaseType.Attack:
                    ActionCardType ATKcardType = distance == PlayerDistanceType.Close ? ActionCardType.ATK_Sword : ActionCardType.ATK_Gun;

                    if (cardtotal.TryGetValue(ATKcardType, out tmptotalnum) && tmptotalnum > 0)
                    {
                        result += tmptotalnum;
                        personAbilityNum += PlayerDatas[(int)player].CurrentCharacter.Character.ATK;
                    }

                    break;
                case PhaseType.Defense:
                    if (cardtotal.TryGetValue(ActionCardType.DEF, out tmptotalnum))
                    {
                        result += tmptotalnum;
                    }

                    personAbilityNum += PlayerDatas[(int)player].CurrentCharacter.Character.DEF;

                    break;
            }

            #region 執行指令-攻擊/防禦階段角色白值能力對骰數變化量控制(EventPersonAbilityDiceChange)
            if ((phase == PhaseType.Attack && result > 0) || phase == PhaseType.Defense)
            {
                PlayerDatas[(int)player].SC_EventPersonAbilityDiceChangeRecord.RecordValue = false;

                foreach (var userPlayerRelativeType in System.Enum.GetValues<UserPlayerRelativeType>())
                {
                    foreach (var skillType in System.Enum.GetValues<SkillType>())
                    {
                        foreach (var record in PlayerDatas[(int)player].SC_EventPersonAbilityDiceChangeRecord.MainProperty[userPlayerRelativeType][skillType])
                        {
                            if (PlayerDatas[(int)player].SC_EventPersonAbilityDiceChangeRecord.RecordValue &&
                                record.Type != NumberChangeRecordThreeVersionType.Assign)
                                continue;

                            switch (record.Type)
                            {
                                case NumberChangeRecordThreeVersionType.Addition:
                                    personAbilityNum += record.Value;
                                    break;
                                case NumberChangeRecordThreeVersionType.Subtraction:
                                    personAbilityNum -= record.Value;
                                    break;
                                case NumberChangeRecordThreeVersionType.Assign:
                                    PlayerDatas[(int)player].SC_EventPersonAbilityDiceChangeRecord.RecordValue = true;
                                    personAbilityNum = record.Value;
                                    break;
                            }
                        }
                    }
                }

                result += personAbilityNum;
            }
            #endregion

            #region 執行指令-攻擊/防禦階段系統骰數變化量控制(EventTotalDiceChange)
            PlayerDatas[(int)player].SC_EventTotalDiceChangeRecord.RecordValue = false;

            foreach (var userPlayerRelativeType in System.Enum.GetValues<UserPlayerRelativeType>())
            {
                foreach (var skillType in System.Enum.GetValues<SkillType>())
                {
                    foreach (var record in PlayerDatas[(int)player].SC_EventTotalDiceChangeRecord.MainProperty[userPlayerRelativeType][skillType])
                    {
                        if (PlayerDatas[(int)player].SC_EventTotalDiceChangeRecord.RecordValue &&
                            record.Type != NumberChangeRecordSixVersionType.Assign)
                            continue;

                        switch (record.Type)
                        {
                            case NumberChangeRecordSixVersionType.Addition:
                                result += record.Value;
                                break;
                            case NumberChangeRecordSixVersionType.Subtraction:
                                result -= record.Value;
                                break;
                            case NumberChangeRecordSixVersionType.Multiplication:
                                result *= record.Value;
                                break;
                            case NumberChangeRecordSixVersionType.Division_Floor:
                                result = (int)Math.Floor((double)result / record.Value);
                                break;
                            case NumberChangeRecordSixVersionType.Division_Ceiling:
                                result = (int)Math.Ceiling((double)result / record.Value);
                                break;
                            case NumberChangeRecordSixVersionType.Assign:
                                PlayerDatas[(int)player].SC_EventTotalDiceChangeRecord.RecordValue = true;
                                result = record.Value;
                                break;
                        }
                    }
                }
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 更新玩家總骰數資料
        /// </summary>
        /// <param name="startPlayer">開始玩家方</param>
        /// <param name="phase">開始玩家階段</param>
        private void UpdatePlayerDiceTotalNumber(UserPlayerType startPlayer, PhaseType phase)
        {
            UserPlayerType playerType_Opponent = startPlayer == UserPlayerType.Player1 ? UserPlayerType.Player2 : UserPlayerType.Player1;
            PhaseType phaseType_Opponent = phase == PhaseType.Attack ? PhaseType.Defense : PhaseType.Attack;

            foreach (var userPlayerRelativeType in System.Enum.GetValues<UserPlayerRelativeType>())
            {
                foreach (var skillType in System.Enum.GetValues<SkillType>())
                {
                    PlayerDatas[(int)startPlayer].SC_EventPersonAbilityDiceChangeRecord.MainProperty[userPlayerRelativeType][skillType].Clear();
                    PlayerDatas[(int)playerType_Opponent].SC_EventPersonAbilityDiceChangeRecord.MainProperty[userPlayerRelativeType][skillType].Clear();
                    PlayerDatas[(int)startPlayer].SC_EventTotalDiceChangeRecord.MainProperty[userPlayerRelativeType][skillType].Clear();
                    PlayerDatas[(int)playerType_Opponent].SC_EventTotalDiceChangeRecord.MainProperty[userPlayerRelativeType][skillType].Clear();
                }
            }

            //執行階段(45)
            SkillAdapter.StageStart(45, startPlayer, true, true);

            PlayerDatas[(int)startPlayer].DiceTotal = GetPlayerDiceTotalNumber(startPlayer, phase, PlayerDistance);
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

                while (CardDecks[cardtype].Count > 0)
                {
                    var tmpCard = CardDecks[cardtype].First();

                    if (tmpCard.Value.GetType().Equals(typeof(EventCard)))
                    {
                        tmpCard.Value.Location = ActionCardLocation.Fold;
                        tmpCard.Value.Owner = ActionCardOwner.System;

                        DeckCardMove(tmpCard.Value, cardtype, CardDeckType.Fold);
                    }
                    else
                    {
                        tmpCard.Value.Location = ActionCardLocation.Graveyard;
                        tmpCard.Value.Owner = ActionCardOwner.System;

                        DeckCardMove(tmpCard.Value, cardtype, CardDeckType.Graveyard);
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
        private static int DiceAction(int diceTotal)
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
        /// <returns>是否通過存活確認/是否已進行變更角色動作</returns>
        private (bool, bool) PlayerCharacterHPCheck()
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
                        return (true, true);
                    }
                    else
                    {
                        return (false, false);
                    }
                }
            }
            return (true, false);
        }

        /// <summary>
        /// 角色傷害執行
        /// </summary>
        private void CharacterHPDamage(CharacterHPDamageDataModel data)
        {
            var characterData = PlayerDatas[(int)data.Player].GetCharacterData(data.CharacterVBEID) ?? throw new Exception("characterVBEID null reference");

            if (characterData.CurrentHP > 0)
            {
                int origHP = characterData.CurrentHP;
                int setDamageNumber = data.DamageNumber;
                bool tmpEventBloodActionDeathTypeChange = false;

                if (data.IsCallEvent)
                {
                    SkillCommandProxy.EventBloodActionRecord.Add(SkillAdapter.StageCallCount + 1, new());

                    SkillAdapter.StageStart(46, data.Player, true, true,
                    [
                        ((int)data.Player).ToString(),
                    PlayerDatas[(int)data.Player].GetCharacterDataIndex(data.CharacterVBEID).ToString()!,
                    ((int)data.DamageType).ToString(),
                    data.DamageNumber.ToString(),
                    ((int)data.TriggerPlayerType).ToString(),
                    ((int)data.TriggerSkillType).ToString()
                    ]);

                    var tmpRecord = SkillCommandProxy.EventBloodActionRecord[SkillAdapter.StageCallCount + 1];
                    SkillCommandProxy.EventBloodActionRecord.Remove(SkillAdapter.StageCallCount + 1);

                    if (tmpRecord.MainProperty.Item1) //EventBloodActionOff
                    {
                        return;
                    }
                    else if (tmpRecord.MainProperty.Item2) //EventBloodActionChange
                    {
                        switch (tmpRecord.RecordValue.Item1)
                        {
                            case NumberChangeRecordThreeVersionType.Addition:
                                if (data.DamageType != CharacterHPDamageType.Death)
                                {
                                    setDamageNumber += tmpRecord.RecordValue.Item2;
                                }
                                break;
                            case NumberChangeRecordThreeVersionType.Subtraction:
                                if (data.DamageType != CharacterHPDamageType.Death)
                                {
                                    setDamageNumber -= tmpRecord.RecordValue.Item2;
                                }
                                break;
                            case NumberChangeRecordThreeVersionType.Assign:
                                if (data.DamageType == CharacterHPDamageType.Death)
                                    tmpEventBloodActionDeathTypeChange = true;

                                setDamageNumber = tmpRecord.RecordValue.Item2;
                                break;
                        }
                    }
                }

                if (setDamageNumber < 0) setDamageNumber = 0;

                if (data.DamageType == CharacterHPDamageType.Death && !tmpEventBloodActionDeathTypeChange)
                {
                    characterData.CurrentHP = 0;
                }
                else
                {
                    characterData.CurrentHP -= setDamageNumber;
                    if (characterData.CurrentHP < 0) characterData.CurrentHP = 0;
                }

                if (origHP - characterData.CurrentHP > 0)
                {
                    MultiUIAdapter.UpdateDataRelative(UpdateDataRelativeType.CharacterHPDamage, data.Player, origHP - characterData.CurrentHP, data.CharacterVBEID);
                }
            }
        }

        /// <summary>
        /// 角色回復執行
        /// </summary>
        /// <param name="player">玩家</param>
        /// <param name="characterVBEID">角色VBEID</param>
        /// <param name="healNumber">回復數</param>
        private void CharacterHPHeal(CharacterHPHealDataModel data)
        {
            var characterData = PlayerDatas[(int)data.Player].GetCharacterData(data.CharacterVBEID) ?? throw new Exception("characterVBEID null reference");
            if (characterData.CurrentHP > 0 && characterData.CurrentHP < characterData.Character.HP)
            {
                int origHP = characterData.CurrentHP;
                int setHealNumber = data.HealNumber;

                if (data.IsCallEvent)
                {
                    SkillCommandProxy.EventHealActionRecord.Add(SkillAdapter.StageCallCount + 1, new());

                    SkillAdapter.StageStart(48, data.Player, true, true,
                    [
                        ((int)data.Player).ToString(),
                        PlayerDatas[(int)data.Player].GetCharacterDataIndex(data.CharacterVBEID).ToString()!,
                        data.HealNumber.ToString(),
                        ((int)data.TriggerPlayerType).ToString(),
                        ((int)data.TriggerSkillType).ToString()
                    ]);

                    var tmpRecord = SkillCommandProxy.EventHealActionRecord[SkillAdapter.StageCallCount + 1];
                    SkillCommandProxy.EventHealActionRecord.Remove(SkillAdapter.StageCallCount + 1);

                    if (tmpRecord.MainProperty.Item1) //EventHealActionOff
                    {
                        return;
                    }
                    else if (tmpRecord.MainProperty.Item2) //EventHealActionChange
                    {
                        switch (tmpRecord.RecordValue.Item1)
                        {
                            case NumberChangeRecordThreeVersionType.Addition:
                                setHealNumber += tmpRecord.RecordValue.Item2;
                                break;
                            case NumberChangeRecordThreeVersionType.Subtraction:
                                setHealNumber -= tmpRecord.RecordValue.Item2;
                                break;
                            case NumberChangeRecordThreeVersionType.Assign:
                                setHealNumber = tmpRecord.RecordValue.Item2;
                                break;
                        }
                    }
                }

                if (setHealNumber < 0) setHealNumber = 0;

                characterData.CurrentHP += setHealNumber;
                if (characterData.CurrentHP > characterData.Character.HP) characterData.CurrentHP = characterData.Character.HP;

                if (characterData.CurrentHP - origHP > 0)
                {
                    MultiUIAdapter.UpdateDataRelative(UpdateDataRelativeType.CharacterHPHeal, data.Player, characterData.CurrentHP - origHP, data.CharacterVBEID);
                }
            }
        }

        /// <summary>
        /// 變更玩家雙方場上距離
        /// </summary>
        /// <param name="newDistance">新設定距離</param>
        /// <param name="isCallEvent">是否觸發執行階段事件(47)</param>
        /// <param name="triggerPlayer">觸發事件方</param>
        private void ChangePlayerDistance(PlayerDistanceType newDistance, bool isCallEvent, CommandPlayerType triggerPlayer)
        {
            m_playerDistance.RecordValue = false;
            if (isCallEvent)
            {
                SkillAdapter.StageStart(47, triggerPlayer == CommandPlayerType.System ? AttackPhaseFirst : triggerPlayer.ToUserPlayerType()!.Value, true, true,
                [
                    ((int)PlayerDistance.ToCommandPlayerDistanceType()).ToString(),
                    ((int)newDistance.ToCommandPlayerDistanceType()).ToString(),
                    ((int)triggerPlayer).ToString(),
                ]);
            }

            if (!m_playerDistance.RecordValue)
            {
                m_playerDistance.MainProperty = newDistance;

                MultiUIAdapter.UpdateData_All(new()
                {
                    Type = UpdateDataType.PlayerDistanceType,
                    Value = (int)PlayerDistance.ToCommandPlayerDistanceType()
                });
            }
        }

        private void UIUpdateBuffData()
        {
            Dictionary<UserPlayerType, Dictionary<string, List<BuffDataBaseModel>>> data = [];
            foreach (var player in System.Enum.GetValues<UserPlayerType>())
            {
                Dictionary<string, List<BuffDataBaseModel>> tmpdata = [];
                foreach (var characterData in PlayerDatas[(int)player].CharacterDatas)
                {
                    tmpdata.Add(characterData.Character.VBEID, characterData.BuffDatas.Select(x => new BuffDataBaseModel
                    {
                        Identifier = x.Buff.Identifier,
                        Value = x.Value,
                        Total = x.Total
                    }).ToList());
                }
                data.Add(player, tmpdata);
            }

            MultiUIAdapter.BuffDataUpdate(data[UserPlayerType.Player1], data[UserPlayerType.Player2]);
        }
    }
}
