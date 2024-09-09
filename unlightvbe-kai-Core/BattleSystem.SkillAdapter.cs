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
    public partial class BattleSystem
    {
        /// <summary>
        /// 技能執行器類別
        /// </summary>
        protected class SkillAdapterClass : ISkillAdapter
        {
            private BattleSystem BattleSystem;
            private PlayerData[] playerDatas;
            private ISkillCommandProxy SkillCommandProxy;

            public SkillAdapterClass(BattleSystem battleSystem)
            {
                BattleSystem = battleSystem;
                playerDatas = battleSystem.PlayerDatas;
            }

            public void SetSkillCommandProxy(ISkillCommandProxy skillCommandProxy)
            {
                SkillCommandProxy = skillCommandProxy;
            }

            /// <summary>
            /// 開始執行階段
            /// </summary>
            /// <param name="stageNum">執行階段號</param>
            /// <param name="startPlayer">開始玩家方</param>
            /// <param name="isAuthMode">是否為驗證模式</param>
            /// <param name="isMulti">是否為雙方玩家執行</param>
            public void StageStart(int stageNum, UserPlayerType startPlayer, bool isAuthMode, bool isMulti)
            {
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
                        int skillindex = 0;

                        foreach (var skill in characterData.Character.ActiveSkills)
                        {
                            if (skill != null && skill.StageNumber.Any(x => x == stageNum) && (characterData.ActiveSkillIsActivate[skillindex] || !isAuthMode))
                            {
                                var commandresult = skill.Function.Invoke(GetActiveSkillArgsModel(player, j));
                                SkillCommandProxy.ExecuteCommands(stageNum, player, SkillType.ActiveSkill, commandresult);
                            }
                            skillindex++;
                        }
                    }
                }
            }

            /// <summary>
            /// 取得技能引數資料傳遞物件(主動技能)
            /// </summary>
            /// <returns></returns>
            private ActiveSkillArgsModel GetActiveSkillArgsModel(UserPlayerType startPlayer, int characterBattleIndex)
            {
                var result = new ActiveSkillArgsModel();
                var oppenentPlayer = startPlayer.GetOppenentPlayer();

                result.CharacterHP = new int[2][];
                result.CharacterHP[0] = playerDatas[(int)startPlayer].CharacterDatas.Select(x => x.CurrentHP).ToArray();
                result.CharacterHP[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Select(x => x.CurrentHP).ToArray();

                result.CharacterHPMAX = new int[2][];
                result.CharacterHPMAX[0] = playerDatas[(int)startPlayer].CharacterDatas.Select(x => x.Character.HP).ToArray();
                result.CharacterHPMAX[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Select(x => x.Character.HP).ToArray();

                result.CharacterBattleIndex = characterBattleIndex;

                result.HoldCardCount = new int[2];
                result.HoldCardCount[0] = BattleSystem.CardDecks[BattleSystem.GetCardDeckType(startPlayer, ActionCardLocation.Hold)].Count;
                result.HoldCardCount[1] = BattleSystem.CardDecks[BattleSystem.GetCardDeckType(oppenentPlayer, ActionCardLocation.Hold)].Count;

                result.PlayCardCount = new int[2];
                result.PlayCardCount[0] = BattleSystem.CardDecks[BattleSystem.GetCardDeckType(startPlayer, ActionCardLocation.Play)].Count;
                result.PlayCardCount[1] = BattleSystem.CardDecks[BattleSystem.GetCardDeckType(oppenentPlayer, ActionCardLocation.Play)].Count;

                result.DiceTotal = new int[2];
                result.DiceTotal[0] = playerDatas[(int)startPlayer].DiceTotal;
                result.DiceTotal[1] = playerDatas[(int)oppenentPlayer].DiceTotal;

                result.DiceTrue = new int[2];
                result.DiceTrue[0] = BattleSystem.DiceTrue[(int)startPlayer];
                result.DiceTrue[1] = BattleSystem.DiceTrue[(int)oppenentPlayer];

                result.DiceTrueTotal = BattleSystem.DiceTrueTotal;

                result.PlayerDistance = BattleSystem.PlayerDistance;

                result.TurnNum = BattleSystem.TurnNum;

                result.DeckNum = BattleSystem.CardDecks[CardDeckType.Deck].Count;

                result.Phase = BattleSystem.Phase[(int)startPlayer];

                result.AttackPhaseFirst = BattleSystem.AttackPhaseFirst.ToRelative(startPlayer);

                result.CharacterCount = new int[2];
                result.CharacterCount[0] = playerDatas[(int)startPlayer].CharacterDatas.Count;
                result.CharacterCount[1] = playerDatas[(int)oppenentPlayer].CharacterDatas.Count;

                result.PlayerHoldMaxCount = new int[2];
                result.PlayerHoldMaxCount[0] = playerDatas[(int)startPlayer].HoldMaxCount;
                result.PlayerHoldMaxCount[1] = playerDatas[(int)oppenentPlayer].HoldMaxCount;

                result.ActionCardTotal = new Dictionary<ActionCardType, int>[2];
                result.ActionCardTotal[0] = BattleSystem.GetCardTotalNumber(startPlayer, ActionCardLocation.Play);
                result.ActionCardTotal[1] = BattleSystem.GetCardTotalNumber(oppenentPlayer, ActionCardLocation.Play);

                result.CardDecks = BattleSystem.CardDecks.SelectMany(x => new[]
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

                result.CardDeckIndex = BattleSystem.CardDeckIndex.SelectMany(x => new[]
                {(
                    x.Key, x.Value.ToRelative(startPlayer)
                )}
                ).ToDictionary(x => x.Key, x => x.Item2);

                return result;
            }
        }
    }
}
