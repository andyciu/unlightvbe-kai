using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 技能執行器類別
        /// </summary>
        protected class SkillAdapterClass(BattleSystem battleSystem) : ISkillAdapter
        {
            private PlayerData[] playerDatas = battleSystem.PlayerDatas;
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

                    //ActiveSkill
                    for (int j = 0; j < playerDatas[(int)player].CharacterDatas.Count; j++)
                    {
                        var characterData = playerDatas[(int)player].CharacterDatas[j];
                        int skillIndex = 0;

                        foreach (var skill in characterData.Character.ActiveSkills)
                        {
                            if (skill != null && skill.StageNumber.Any(x => x == stageNum) && (characterData.ActiveSkillIsActivate[skillIndex] || !isAuthMode))
                            {
                                var commandresult = skill.Function.Invoke(GetActiveSkillArgsModel(skill, player, j, skillIndex, stageNum, stageMessage));
                                var data = new SkillCommandProxyExecueCommandDataModel
                                {
                                    StageNum = stageNum,
                                    Player = player,
                                    SkillType = SkillType.ActiveSkill,
                                    SkillIndex = skillIndex,
                                    SkillID = skill.SkillID,
                                    StageType = GetSkillStageType(stageNum),
                                    CharacterBattleIndex = j,
                                    IsAuthMode = isAuthMode
                                };
                                battleSystem.SkillCommandProxy.ExecuteCommands(data, commandresult);
                            }
                            skillIndex++;
                        }
                    }
                }

                StageCallCount--;
            }

            /// <summary>
            /// 開始執行階段程序(指定單方個體)
            /// </summary>
            /// <param name="stageNum">執行階段號</param>
            /// <param name="player">指定玩家方</param>
            /// <param name="characterBattleIndex">指定角色Index</param>
            /// <param name="skillType">指定技能體系</param>
            /// <param name="skillIndex">指定技能Index</param>
            public void StageStartSkillOnly(int stageNum, UserPlayerType player, int characterBattleIndex, SkillType skillType, int skillIndex)
            {
                this.StageStartSkillOnly(stageNum, player, characterBattleIndex, skillType, skillIndex, null);
            }

            /// <summary>
            /// 開始執行階段程序(指定單方個體)
            /// </summary>
            /// <param name="stageNum">執行階段號</param>
            /// <param name="player">指定玩家方</param>
            /// <param name="characterBattleIndex">指定角色Index</param>
            /// <param name="skillType">指定技能體系</param>
            /// <param name="skillIndex">指定技能Index</param>
            /// <param name="stageMessage">執行階段多用途紀錄資訊</param>
            public void StageStartSkillOnly(int stageNum, UserPlayerType player, int characterBattleIndex, SkillType skillType, int skillIndex, string[]? stageMessage)
            {
                StageCallCount++;

                var characterData = playerDatas[(int)player].CharacterDatas[characterBattleIndex];
                switch (skillType)
                {
                    case SkillType.ActiveSkill:
                        var skill = characterData.Character.ActiveSkills[skillIndex];
                        if (skill != null && skill.StageNumber.Any(x => x == stageNum) && (characterData.ActiveSkillIsActivate[skillIndex]))
                        {
                            var commandresult = skill.Function.Invoke(GetActiveSkillArgsModel(skill, player, characterBattleIndex, skillIndex, stageNum, stageMessage));
                            var data = new SkillCommandProxyExecueCommandDataModel
                            {
                                StageNum = stageNum,
                                Player = player,
                                SkillType = SkillType.ActiveSkill,
                                SkillIndex = skillIndex,
                                SkillID = skill.SkillID,
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
            /// 取得技能引數資料傳遞物件(主動技能)
            /// </summary>
            /// <returns></returns>
            private ActiveSkillArgsModel GetActiveSkillArgsModel(Skill<ActiveSkill> skill, UserPlayerType startPlayer, int characterBattleIndex, int skillIndex, int stageNum, string[]? stageMessage)
            {
                var result = new ActiveSkillArgsModel();
                var oppenentPlayer = startPlayer.GetOppenentPlayer();

                result.StageNum = stageNum;

                result.CharacterHP = new int[2][];
                result.CharacterHP[0] = playerDatas[(int)startPlayer].CharacterDatas.Select(x => x.CurrentHP).ToArray();
                result.CharacterHP[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Select(x => x.CurrentHP).ToArray();

                result.CharacterHPMAX = new int[2][];
                result.CharacterHPMAX[0] = playerDatas[(int)startPlayer].CharacterDatas.Select(x => x.Character.HP).ToArray();
                result.CharacterHPMAX[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Select(x => x.Character.HP).ToArray();

                result.CharacterBattleIndex = characterBattleIndex;

                result.HoldCardCount = new int[2];
                result.HoldCardCount[0] = battleSystem.CardDecks[GetCardDeckType(startPlayer, ActionCardLocation.Hold)].Count;
                result.HoldCardCount[1] = battleSystem.CardDecks[GetCardDeckType(oppenentPlayer, ActionCardLocation.Hold)].Count;

                result.PlayCardCount = new int[2];
                result.PlayCardCount[0] = battleSystem.CardDecks[GetCardDeckType(startPlayer, ActionCardLocation.Play)].Count;
                result.PlayCardCount[1] = battleSystem.CardDecks[GetCardDeckType(oppenentPlayer, ActionCardLocation.Play)].Count;

                result.DiceTotal = new int[2];
                result.DiceTotal[0] = playerDatas[(int)startPlayer].DiceTotal;
                result.DiceTotal[1] = playerDatas[(int)oppenentPlayer].DiceTotal;

                result.DiceTrue = new int[2];
                result.DiceTrue[0] = battleSystem.DiceTrue[(int)startPlayer];
                result.DiceTrue[1] = battleSystem.DiceTrue[(int)oppenentPlayer];

                result.DiceTrueTotal = battleSystem.DiceTrueTotal;

                result.CharacterActiveSkillIsActivate = new bool[2][];
                result.CharacterActiveSkillIsActivate[0] = playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].ActiveSkillIsActivate.ToArray();
                result.CharacterActiveSkillIsActivate[1] = playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].ActiveSkillIsActivate.ToArray();

                result.CharacterPassiveSkillIsActivate = new bool[2][];
                result.CharacterPassiveSkillIsActivate[0] = playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].PassiveSkillIsActivate.ToArray();
                result.CharacterPassiveSkillIsActivate[1] = playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].PassiveSkillIsActivate.ToArray();

                result.CharacterActiveSkillTurnOnCount = new int[2][];
                result.CharacterActiveSkillTurnOnCount[0] = playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].ActiveSkillTurnOnCount.ToArray();
                result.CharacterActiveSkillTurnOnCount[1] = playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].ActiveSkillTurnOnCount.ToArray();

                result.CharacterPassiveSkillTurnOnCount = new int[2][];
                result.CharacterPassiveSkillTurnOnCount[0] = playerDatas[(int)startPlayer].CharacterDatas[characterBattleIndex].PassiveSkillTurnOnCount.ToArray();
                result.CharacterPassiveSkillTurnOnCount[1] = playerDatas[(int)oppenentPlayer].CharacterDatas[characterBattleIndex].PassiveSkillTurnOnCount.ToArray();

                result.PlayerDistance = battleSystem.PlayerDistance;

                result.TurnNum = battleSystem.TurnNum;

                result.DeckNum = battleSystem.CardDecks[CardDeckType.Deck].Count;

                result.Phase = battleSystem.Phase[(int)startPlayer];

                result.AttackPhaseFirst = battleSystem.AttackPhaseFirst.ToRelative(startPlayer);

                result.CharacterCount = new int[2];
                result.CharacterCount[0] = playerDatas[(int)startPlayer].CharacterDatas.Count;
                result.CharacterCount[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Count;

                result.PlayerHoldMaxCount = new int[2];
                result.PlayerHoldMaxCount[0] = playerDatas[(int)startPlayer].HoldMaxCount;
                result.PlayerHoldMaxCount[1] = playerDatas[(int)oppenentPlayer].HoldMaxCount;

                result.ActionCardTotal = new Dictionary<ActionCardType, int>[2];
                result.ActionCardTotal[0] = battleSystem.GetCardTotalNumber(startPlayer, ActionCardLocation.Play);
                result.ActionCardTotal[1] = battleSystem.GetCardTotalNumber(oppenentPlayer, ActionCardLocation.Play);

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

                result.StageMessage = ProcessStageMessage(stageNum, startPlayer, stageMessage);

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
                }

                return resultMessage;
            }
        }
    }
}
