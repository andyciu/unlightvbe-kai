using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.Skill;
using unlightvbe_kai_core.Models.SkillArgs;
using unlightvbe_kai_core.Models.UserInterface;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 技能執行器類別
        /// </summary>
        protected class SkillAdapterClass(BattleSystem battleSystem) : ISkillAdapter
        {
            private readonly PlayerData[] playerDatas = battleSystem.PlayerDatas;
            /// <summary>
            /// 執行階段階層計數
            /// </summary>
            public int StageCallCount { get; private set; }
            /// <summary>
            /// 開始執行階段程序
            /// </summary>
            /// <param name="stageNum">執行階段號</param>
            /// <param name="startPlayer">開始玩家方</param>
            /// <param name="isAuthMode">是否為驗證模式</param>
            /// <param name="isMulti">是否為雙方玩家執行</param>
            public void StageStart(int stageNum, UserPlayerType startPlayer, bool isAuthMode, bool isMulti)
            {
                this.StageStart(stageNum, startPlayer, isAuthMode, isMulti, null);
            }

            /// <summary>
            /// 開始執行階段程序
            /// </summary>
            /// <param name="stageNum">執行階段號</param>
            /// <param name="startPlayer">開始玩家方</param>
            /// <param name="isAuthMode">是否為驗證模式</param>
            /// <param name="isMulti">是否為雙方玩家執行</param>
            /// <param name="stageMessage">執行階段多用途紀錄資訊</param>
            public void StageStart(int stageNum, UserPlayerType startPlayer, bool isAuthMode, bool isMulti, string[]? stageMessage)
            {
                StageCallCount++;

                for (int i = 0; i < 2; i++)
                {
                    UserPlayerType player;
                    if (i == 0) player = startPlayer;
                    else
                    {
                        if (!isMulti) break;
                        else player = startPlayer.GetOppenentPlayer();
                    }

                    for (int j = 0; j < playerDatas[(int)player].CharacterDatas.Count; j++)
                    {
                        var characterData = playerDatas[(int)player].CharacterDatas[j];
                        int skillIndex = 0;

                        //ActiveSkill
                        foreach (var skill in characterData.Character.ActiveSkills)
                        {
                            if (skill != null && (j == 0 || skill.IsCallOnBackground) && skill.StageNumber.Any(x => x == stageNum) && (characterData.ActiveSkillIsActivate[skillIndex] || !isAuthMode))
                            {
                                var commandresult = skill.Function.Invoke(GetActiveSkillArgsModel(skill, player, j, skillIndex, stageNum, stageMessage));
                                var data = new SkillCommandProxyExecueCommandDataModel
                                {
                                    StageNum = stageNum,
                                    Player = player,
                                    SkillType = SkillType.ActiveSkill,
                                    SkillIndex = skillIndex,
                                    SkillID = skill.Identifier,
                                    StageType = GetSkillStageType(stageNum),
                                    CharacterBattleIndex = j,
                                    IsAuthMode = isAuthMode
                                };
                                battleSystem.SkillCommandProxy.ExecuteCommands(data, commandresult);
                            }
                            skillIndex++;
                        }

                        //PassiveSkill
                        skillIndex = 0;
                        foreach (var skill in characterData.Character.PassiveSkills)
                        {
                            if (skill != null && skill.StageNumber.Any(x => x == stageNum) &&
                                (characterData.PassiveSkillIsActivate[skillIndex] || !isAuthMode || GetSkillStageType(stageNum) == SkillStageType.Normal))
                            {
                                var commandresult = skill.Function.Invoke(GetPassiveSkillArgsModel(player, j, skillIndex, stageNum, stageMessage));
                                var data = new SkillCommandProxyExecueCommandDataModel
                                {
                                    StageNum = stageNum,
                                    Player = player,
                                    SkillType = SkillType.PassiveSkill,
                                    SkillIndex = skillIndex,
                                    SkillID = skill.Identifier,
                                    StageType = GetSkillStageType(stageNum),
                                    CharacterBattleIndex = j,
                                    IsAuthMode = isAuthMode
                                };
                                battleSystem.SkillCommandProxy.ExecuteCommands(data, commandresult);
                            }
                            skillIndex++;
                        }

                        //Buff
                        foreach (var buffData in characterData.BuffDatas.ToList())
                        {
                            if (buffData != null && buffData.Buff.StageNumber.Any(x => x == stageNum))
                            {
                                var commandresult = buffData.Buff.Function.Invoke(GetBuffArgsModel(player, j, buffData, stageNum, stageMessage));
                                var data = new SkillCommandProxyExecueCommandDataModel
                                {
                                    StageNum = stageNum,
                                    Player = player,
                                    SkillType = SkillType.Buff,
                                    SkillIndex = -1,
                                    SkillID = buffData.Buff.Identifier,
                                    StageType = GetSkillStageType(stageNum),
                                    CharacterBattleIndex = j,
                                    IsAuthMode = isAuthMode
                                };
                                battleSystem.SkillCommandProxy.ExecuteCommands(data, commandresult);
                            }
                        }
                    }
                }

                StageCallCount--;
            }

            /// <summary>
            /// 開始執行階段程序[主動/被動技能](指定單方個體)
            /// </summary>
            /// <param name="stageNum">執行階段號</param>
            /// <param name="player">指定玩家方</param>
            /// <param name="characterBattleIndex">指定角色Index</param>
            /// <param name="skillType">指定技能體系</param>
            /// <param name="skillIndex">指定技能Index</param>
            /// <param name="stageMessage">執行階段多用途紀錄資訊</param>
            public void StageStartSkillOnly_Active_Passive(int stageNum, UserPlayerType player, int characterBattleIndex, SkillType skillType, int skillIndex, string[]? stageMessage)
            {
                if (skillType != SkillType.ActiveSkill && skillType != SkillType.PassiveSkill) return;

                StageCallCount++;

                var characterData = playerDatas[(int)player].CharacterDatas[characterBattleIndex];
                switch (skillType)
                {
                    case SkillType.ActiveSkill:
                        var tmpActiveSkill = characterData.Character.ActiveSkills[skillIndex];
                        if (tmpActiveSkill != null && (characterBattleIndex == 0 || tmpActiveSkill.IsCallOnBackground)  && tmpActiveSkill.StageNumber.Any(x => x == stageNum) && (characterData.ActiveSkillIsActivate[skillIndex]))
                        {
                            var commandresult = tmpActiveSkill.Function.Invoke(GetActiveSkillArgsModel(tmpActiveSkill, player, characterBattleIndex, skillIndex, stageNum, stageMessage));
                            var data = new SkillCommandProxyExecueCommandDataModel
                            {
                                StageNum = stageNum,
                                Player = player,
                                SkillType = SkillType.ActiveSkill,
                                SkillIndex = skillIndex,
                                SkillID = tmpActiveSkill.Identifier,
                                StageType = GetSkillStageType(stageNum),
                                CharacterBattleIndex = characterBattleIndex,
                                IsAuthMode = true
                            };
                            battleSystem.SkillCommandProxy.ExecuteCommands(data, commandresult);
                        }
                        break;
                    case SkillType.PassiveSkill:
                        var tmpPassiveSkill = characterData.Character.PassiveSkills[skillIndex];

                        if (tmpPassiveSkill != null && tmpPassiveSkill.StageNumber.Any(x => x == stageNum) &&
                                (characterData.PassiveSkillIsActivate[skillIndex] || GetSkillStageType(stageNum) == SkillStageType.Normal))
                        {
                            var commandresult = tmpPassiveSkill.Function.Invoke(GetPassiveSkillArgsModel(player, characterBattleIndex, skillIndex, stageNum, stageMessage));
                            var data = new SkillCommandProxyExecueCommandDataModel
                            {
                                StageNum = stageNum,
                                Player = player,
                                SkillType = SkillType.PassiveSkill,
                                SkillIndex = skillIndex,
                                SkillID = tmpPassiveSkill.Identifier,
                                StageType = GetSkillStageType(stageNum),
                                CharacterBattleIndex = characterBattleIndex,
                                IsAuthMode = true
                            };
                            battleSystem.SkillCommandProxy.ExecuteCommands(data, commandresult);
                        }
                        break;
                }

                StageCallCount--;
            }

            /// <summary>
            /// 開始執行階段程序[異常狀態](指定單方個體)
            /// </summary>
            /// <param name="stageNum">執行階段號</param>
            /// <param name="player">指定玩家方</param>
            /// <param name="characterBattleIndex">指定角色Index</param>
            /// <param name="buffIdentifier">技能唯一識別碼</param>
            /// <param name="stageMessage">執行階段多用途紀錄資訊</param>
            public void StageStartSkillOnly_Buff(int stageNum, UserPlayerType player, int characterBattleIndex, string buffIdentifier, string[]? stageMessage)
            {
                var characterData = playerDatas[(int)player].CharacterDatas[characterBattleIndex];
                if (!characterData.BuffDatas.Any(x => x.Buff.Identifier == buffIdentifier)) return;

                StageCallCount++;

                var buffData = characterData.BuffDatas.First(x => x.Buff.Identifier == buffIdentifier);

                if (buffData != null && buffData.Buff.StageNumber.Any(x => x == stageNum))
                {
                    var commandresult = buffData.Buff.Function.Invoke(GetBuffArgsModel(player, characterBattleIndex, buffData, stageNum, stageMessage));
                    var data = new SkillCommandProxyExecueCommandDataModel
                    {
                        StageNum = stageNum,
                        Player = player,
                        SkillType = SkillType.Buff,
                        SkillIndex = -1,
                        SkillID = buffData.Buff.Identifier,
                        StageType = GetSkillStageType(stageNum),
                        CharacterBattleIndex = characterBattleIndex,
                        IsAuthMode = true
                    };
                    battleSystem.SkillCommandProxy.ExecuteCommands(data, commandresult);
                }

                StageCallCount--;
            }

            /// <summary>
            /// 設定技能引數資料傳遞物件基本資訊
            /// </summary>
            /// <param name="model"></param>
            /// <param name="startPlayer"></param>
            /// <param name="characterBattleIndex"></param>
            /// <param name="stageNum"></param>
            /// <param name="stageMessage"></param>
            /// <returns></returns>
            private SkillArgsModelBase SetSkillArgsModelBaseInformation(SkillArgsModelBase model, UserPlayerType startPlayer, int characterBattleIndex, int stageNum, string[]? stageMessage)
            {
                var oppenentPlayer = startPlayer.GetOppenentPlayer();

                model.StageNum = stageNum;

                model.CharacterHP = new int[2][];
                model.CharacterHP[0] = playerDatas[(int)startPlayer].CharacterDatas.Select(x => x.CurrentHP).ToArray();
                model.CharacterHP[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Select(x => x.CurrentHP).ToArray();

                model.CharacterHPMAX = new int[2][];
                model.CharacterHPMAX[0] = playerDatas[(int)startPlayer].CharacterDatas.Select(x => x.Character.HP).ToArray();
                model.CharacterHPMAX[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Select(x => x.Character.HP).ToArray();

                model.CharacterBattleIndex = characterBattleIndex;

                model.HoldCardCount = new int[2];
                model.HoldCardCount[0] = battleSystem.CardDecks[GetCardDeckType(startPlayer, ActionCardLocation.Hold)].Count;
                model.HoldCardCount[1] = battleSystem.CardDecks[GetCardDeckType(oppenentPlayer, ActionCardLocation.Hold)].Count;

                model.PlayCardCount = new int[2];
                model.PlayCardCount[0] = battleSystem.CardDecks[GetCardDeckType(startPlayer, ActionCardLocation.Play)].Count;
                model.PlayCardCount[1] = battleSystem.CardDecks[GetCardDeckType(oppenentPlayer, ActionCardLocation.Play)].Count;

                model.DiceTotal = new int[2];
                model.DiceTotal[0] = playerDatas[(int)startPlayer].DiceTotal;
                model.DiceTotal[1] = playerDatas[(int)oppenentPlayer].DiceTotal;

                model.DiceTrue = new int[2];
                model.DiceTrue[0] = battleSystem.DiceTrue[(int)startPlayer];
                model.DiceTrue[1] = battleSystem.DiceTrue[(int)oppenentPlayer];
                model.DiceTrueTotal = battleSystem.DiceTrueTotal;

                model.PlayerDistance = battleSystem.PlayerDistance.ToCommandPlayerDistanceType();
                model.TurnNum = battleSystem.TurnNum;
                model.DeckNum = battleSystem.CardDecks[CardDeckType.Deck].Count;
                model.Phase = battleSystem.Phase[(int)startPlayer];
                model.AttackPhaseFirst = battleSystem.AttackPhaseFirst.ToRelative(startPlayer);

                model.CharacterCount = new int[2];
                model.CharacterCount[0] = playerDatas[(int)startPlayer].CharacterDatas.Count;
                model.CharacterCount[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Count;

                model.PlayerHoldMaxCount = new int[2];
                model.PlayerHoldMaxCount[0] = playerDatas[(int)startPlayer].HoldMaxCount;
                model.PlayerHoldMaxCount[1] = playerDatas[(int)oppenentPlayer].HoldMaxCount;

                model.ActionCardTotal = new Dictionary<ActionCardType, int>[2];
                model.ActionCardTotal[0] = battleSystem.GetCardTotalNumber(startPlayer, ActionCardLocation.Play);
                model.ActionCardTotal[1] = battleSystem.GetCardTotalNumber(oppenentPlayer, ActionCardLocation.Play);

                model.StageMessage = ProcessStageMessage(stageNum, startPlayer, stageMessage);

                return model;
            }

            /// <summary>
            /// 取得技能引數資料傳遞物件(主動技能)
            /// </summary>
            /// <returns></returns>
            private ActiveSkillArgsModel GetActiveSkillArgsModel(ActiveSkillModel skill, UserPlayerType startPlayer, int characterBattleIndex, int skillIndex, int stageNum, string[]? stageMessage)
            {
                var result = new ActiveSkillArgsModel();
                var oppenentPlayer = startPlayer.GetOppenentPlayer();

                SetSkillArgsModelBaseInformation(result, startPlayer, characterBattleIndex, stageNum, stageMessage);

                result.CharacterActiveSkillIsActivate = new bool[2][];
                result.CharacterActiveSkillIsActivate[0] = [.. playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].ActiveSkillIsActivate];
                result.CharacterActiveSkillIsActivate[1] = [.. playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].ActiveSkillIsActivate];

                result.CharacterPassiveSkillIsActivate = new bool[2][];
                result.CharacterPassiveSkillIsActivate[0] = [.. playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].PassiveSkillIsActivate];
                result.CharacterPassiveSkillIsActivate[1] = [.. playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].PassiveSkillIsActivate];

                result.CharacterActiveSkillTurnOnCount = new int[2][];
                result.CharacterActiveSkillTurnOnCount[0] = [.. playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].ActiveSkillTurnOnCount];
                result.CharacterActiveSkillTurnOnCount[1] = [.. playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].ActiveSkillTurnOnCount];

                result.CharacterPassiveSkillTurnOnCount = new int[2][];
                result.CharacterPassiveSkillTurnOnCount[0] = [.. playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].PassiveSkillTurnOnCount];
                result.CharacterPassiveSkillTurnOnCount[1] = [.. playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].PassiveSkillTurnOnCount];

                result.CardDecks = battleSystem.CardDecks.SelectMany(x => new[]
                {(
                    x.Key.ToRelative(startPlayer), x.Value.SelectMany(u => new[]
                        {(
                            u.Key, new CardModel
                            {
                                Number = u.Value.Number,
                                UpperType = u.Value.UpperType,
                                UpperNum = u.Value.UpperNum,
                                LowerType = u.Value.LowerType,
                                LowerNum = u.Value.LowerNum,
                                Location = u.Value.Location,
                                Owner = u.Value.Owner.ToRelative(startPlayer),
                                Identifier = u.Value.Identifier,
                                IsReverse = u.Value.IsReverse,
                            }
                        )}
                    ).ToDictionary(c => c.Key, c => c.Item2)
                )}
                ).ToDictionary(n => n.Item1, n => n.Item2);

                result.CardDeckIndex = battleSystem.CardDeckIndex.SelectMany(x => new[]
                {(
                    x.Key, x.Value.ToRelative(startPlayer)
                )}
                ).ToDictionary(x => x.Key, x => x.Item2);

                result.SkillPhase = skill.Phase;
                result.SkillDistance = new(skill.Distance);
                result.SkillCardCondition = new(skill.Cards);
                result.SkillIndex = skillIndex;

                return result;
            }

            /// <summary>
            /// 取得技能引數資料傳遞物件(被動技能)
            /// </summary>
            /// <param name="startPlayer"></param>
            /// <param name="characterBattleIndex"></param>
            /// <param name="skillIndex"></param>
            /// <param name="stageNum"></param>
            /// <param name="stageMessage"></param>
            /// <returns></returns>
            private PassiveSkillArgsModel GetPassiveSkillArgsModel(UserPlayerType startPlayer, int characterBattleIndex, int skillIndex, int stageNum, string[]? stageMessage)
            {
                var result = new PassiveSkillArgsModel();
                var oppenentPlayer = startPlayer.GetOppenentPlayer();

                SetSkillArgsModelBaseInformation(result, startPlayer, characterBattleIndex, stageNum, stageMessage);

                result.CharacterActiveSkillIsActivate = new bool[2][];
                result.CharacterActiveSkillIsActivate[0] = [.. playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].ActiveSkillIsActivate];
                result.CharacterActiveSkillIsActivate[1] = [.. playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].ActiveSkillIsActivate];

                result.CharacterPassiveSkillIsActivate = new bool[2][];
                result.CharacterPassiveSkillIsActivate[0] = [.. playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].PassiveSkillIsActivate];
                result.CharacterPassiveSkillIsActivate[1] = [.. playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].PassiveSkillIsActivate];

                result.CharacterActiveSkillTurnOnCount = new int[2][];
                result.CharacterActiveSkillTurnOnCount[0] = [.. playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].ActiveSkillTurnOnCount];
                result.CharacterActiveSkillTurnOnCount[1] = [.. playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].ActiveSkillTurnOnCount];

                result.CharacterPassiveSkillTurnOnCount = new int[2][];
                result.CharacterPassiveSkillTurnOnCount[0] = [.. playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].PassiveSkillTurnOnCount];
                result.CharacterPassiveSkillTurnOnCount[1] = [.. playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].PassiveSkillTurnOnCount];

                result.CardDecks = battleSystem.CardDecks.SelectMany(x => new[]
                {(
                    x.Key.ToRelative(startPlayer), x.Value.SelectMany(u => new[]
                        {(
                            u.Key, new CardModel
                            {
                                Number = u.Value.Number,
                                UpperType = u.Value.UpperType,
                                UpperNum = u.Value.UpperNum,
                                LowerType = u.Value.LowerType,
                                LowerNum = u.Value.LowerNum,
                                Location = u.Value.Location,
                                Owner = u.Value.Owner.ToRelative(startPlayer),
                                Identifier = u.Value.Identifier,
                                IsReverse = u.Value.IsReverse,
                            }
                        )}
                    ).ToDictionary(c => c.Key, c => c.Item2)
                )}
                ).ToDictionary(n => n.Item1, n => n.Item2);

                result.CardDeckIndex = battleSystem.CardDeckIndex.SelectMany(x => new[]
                {(
                    x.Key, x.Value.ToRelative(startPlayer)
                )}
                ).ToDictionary(x => x.Key, x => x.Item2);

                result.SkillIndex = skillIndex;

                return result;
            }

            private BuffArgsModel GetBuffArgsModel(UserPlayerType startPlayer, int characterBattleIndex, BuffData buffData, int stageNum, string[]? stageMessage)
            {
                var result = new BuffArgsModel();

                SetSkillArgsModelBaseInformation(result, startPlayer, characterBattleIndex, stageNum, stageMessage);

                result.BuffValue = buffData.Value;
                result.BuffTotal = buffData.Total;

                return result;
            }

            public static SkillStageType GetSkillStageType(int stageNum)
            {
                int[] stageTypeSpecial = [42, 43, 44, 45, 92, 93, 94, 99];
                int[] stageTypeEvent = [41, 46, 47, 48, 49, 61, 62, 72, 73, 74, 75, 76, 77, 101, 102, 103, 104, 105, 106, 107];

                if (stageTypeSpecial.Any(x => x == stageNum)) return SkillStageType.Special;
                else if (stageTypeEvent.Any(x => x == stageNum)) return SkillStageType.Event;
                else return SkillStageType.Normal;
            }

            /// <summary>
            /// 執行階段多用途紀錄資訊特別處理
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            private static string[]? ProcessStageMessage(int stageNum, UserPlayerType startPlayer, string[]? message)
            {
                if (message == null) return null;

                var resultMessage = new string[message.Length];
                message.CopyTo(resultMessage, 0);

                switch (stageNum)
                {
                    case 46: //PersonBloodControl[DirectDamage/Death]
                        resultMessage[0] = ((int)((UserPlayerType)Convert.ToInt32(message[0])).ToCommandPlayerRelativeTwoVersionType(startPlayer)).ToString();
                        resultMessage[4] = ((int)((CommandPlayerType)Convert.ToInt32(message[4])).ToRelative(startPlayer)).ToString();
                        break;
                    case 47: //BattleMoveControl
                        resultMessage[2] = ((int)((CommandPlayerType)Convert.ToInt32(message[2])).ToRelative(startPlayer)).ToString();
                        break;
                    case 48: //PersonBloodControl[Heal]
                        resultMessage[0] = ((int)((UserPlayerType)Convert.ToInt32(message[0])).ToCommandPlayerRelativeTwoVersionType(startPlayer)).ToString();
                        resultMessage[3] = ((int)((CommandPlayerType)Convert.ToInt32(message[3])).ToRelative(startPlayer)).ToString();
                        break;
                    case 62: //BattleStartDice
                        resultMessage[0] = message[(int)startPlayer.ToRelative(UserPlayerType.Player1)];
                        resultMessage[1] = message[(int)startPlayer.ToRelative(UserPlayerType.Player2)];
                        resultMessage[2] = message[(int)startPlayer.ToRelative(UserPlayerType.Player1) + 2];
                        resultMessage[3] = message[(int)startPlayer.ToRelative(UserPlayerType.Player2) + 2];
                        break;
                    case 76: //人物角色附加狀態增加時
                    case 77: //人物角色附加狀態解除時
                        resultMessage[0] = ((int)((UserPlayerType)Convert.ToInt32(message[0])).ToCommandPlayerRelativeTwoVersionType(startPlayer)).ToString();
                        break;
                }

                return resultMessage;
            }
        }
    }
}
