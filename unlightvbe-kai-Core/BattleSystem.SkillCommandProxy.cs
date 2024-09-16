using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 技能執行指令解釋器類別
        /// </summary>
        protected class SkillCommandProxyClass : ISkillCommand, ISkillCommandProxy
        {
            private BattleSystem BattleSystem;
            private PlayerData[] playerDatas;
            private ISkillAdapter skillAdapter;

            public SkillCommandProxyClass(BattleSystem battleSystem)
            {
                BattleSystem = battleSystem;
                playerDatas = battleSystem.PlayerDatas;
            }

            public void SetSkillAdapter(ISkillAdapter skillAdapter)
            {
                this.skillAdapter = skillAdapter;
            }

            /// <summary>
            /// 執行指令呼叫執行
            /// </summary>
            /// <param name="commandList">指令集合</param>
            public void ExecuteCommands(SkillCommandProxyExecueCommandDataModel data, List<SkillCommandModel> commandList)
            {
                foreach (SkillCommandModel command in commandList)
                {
                    if (!data.IsAuthMode && !CheckCommandIsAllowOnUnAuthMode(command.Type)) continue;

                    var method = this.GetType().GetMethod(command.Type.ToString());
                    if (method != null)
                    {
                        var methodData = new SkillCommandDataModel
                        {
                            Player = data.Player,
                            StageNum = data.StageNum,
                            SkillType = data.SkillType,
                            SkillIndex = data.SkillIndex,
                            SkillID = data.SkillID,
                            StageType = data.StageType,
                            CharacterBattleIndex = data.CharacterBattleIndex,
                            Message = command.Message
                        };
                        method.Invoke(this, new object[] { methodData });
                    }
                }
            }

            /// <summary>
            /// 檢查執行指令是否適用於無驗證模式
            /// </summary>
            /// <returns></returns>
            public static bool CheckCommandIsAllowOnUnAuthMode(SkillCommandType type)
            {
                return type switch
                {
                    SkillCommandType.SkillLineLight => true,
                    SkillCommandType.SkillTurnOnOff => true,
                    _ => false
                };
            }

            /// <summary>
            /// 人物必殺技狀態燈控制(自身)
            /// </summary>
            /// <param name="data"></param>
            public void SkillLineLight(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 1 ||
                    data.StageNum == 45 || data.StageNum == 99 ||
                    data.CharacterBattleIndex > 0)
                    return;

                if (data.SkillType == SkillType.ActiveSkill)
                {
                    BattleSystem.MultiUIAdapter.ActiveSkillLineLight(data.Player, data.SkillIndex,
                    data.Message[0] == "1");
                }
                else if (data.SkillType == SkillType.PassiveSkill)
                {
                    BattleSystem.MultiUIAdapter.UpdateDataRelative(
                        UpdateDataRelativeType.PassiveSkillLineLight,
                        data.Player,
                        data.SkillIndex,
                        (data.Message[0] == "1").ToString());
                }
            }

            /// <summary>
            /// 人物必殺技啟動碼控制(自身)
            /// </summary>
            /// <param name="data"></param>
            public void SkillTurnOnOff(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 1 ||
                    data.StageNum == 45 || data.StageNum == 99 ||
                    (data.SkillType != SkillType.ActiveSkill && data.SkillType != SkillType.PassiveSkill) ||
                    data.CharacterBattleIndex > 0)
                    return;

                if (data.SkillType == SkillType.ActiveSkill)
                {
                    playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex]
                        .ActiveSkillIsActivate[data.SkillIndex] = data.Message[0] == "1";
                }
                else if (data.SkillType == SkillType.PassiveSkill)
                {
                    playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex]
                        .PassiveSkillIsActivate[data.SkillIndex] = data.Message[0] == "1";
                }
            }

            /// <summary>
            /// 人物必殺技狀態燈控制(其他)
            /// </summary>
            /// <param name="data"></param>
            public void SkillLineLightAnother(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 ||
                    (data.StageType != SkillStageType.Normal && data.StageType != SkillStageType.Event) ||
                    (data.SkillType != SkillType.ActiveSkill && data.SkillType != SkillType.PassiveSkill) ||
                    data.CharacterBattleIndex > 0)
                    return;

                switch (data.Message[0])
                {
                    case "1": //ActiveSkill
                        BattleSystem.MultiUIAdapter.ActiveSkillLineLight(data.Player, Convert.ToInt32(data.Message[1]) - 1,
                            data.Message[2] == "1");
                        break;
                    case "2": //PassiveSkill
                        BattleSystem.MultiUIAdapter.UpdateDataRelative(
                            UpdateDataRelativeType.PassiveSkillLineLight,
                            data.Player,
                            Convert.ToInt32(data.Message[1]) - 1,
                            (data.Message[2] == "1").ToString());
                        break;
                }
            }

            /// <summary>
            /// 人物必殺技啟動碼控制(其他)
            /// </summary>
            /// <param name="data"></param>
            public void SkillTurnOnOffAnother(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 ||
                    (data.StageType != SkillStageType.Normal && data.StageType != SkillStageType.Event) ||
                    (data.SkillType != SkillType.ActiveSkill && data.SkillType != SkillType.PassiveSkill))
                    return;

                switch (data.Message[0])
                {
                    case "1": //ActiveSkill
                        playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex]
                            .ActiveSkillIsActivate[Convert.ToInt32(data.Message[1]) - 1] = data.Message[2] == "1";
                        break;
                    case "2": //PassiveSkill
                        playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex]
                            .ActiveSkillIsActivate[Convert.ToInt32(data.Message[1]) - 1] = data.Message[2] == "1";
                        break;
                }
            }

            /// <summary>
            /// 技能動畫圖像執行
            /// </summary>
            /// <param name="data"></param>
            public void SkillAnimateStartPlay(SkillCommandDataModel data)
            {
                if (data.StageType != SkillStageType.Normal || data.SkillType == SkillType.Buff ||
                    data.StageNum == 13 || data.StageNum == 33)
                    return;

                BattleSystem.MultiUIAdapter.ShowSkillAnimate(data.Player, data.SkillID);

                BattleSystem.SkillAdapter.StageStartSkillOnly(61, data.Player, data.CharacterBattleIndex, data.SkillType, data.SkillIndex);
            }

            /// <summary>
            /// 攻擊/防禦階段系統骰數變化量控制
            /// </summary>
            /// <param name="data"></param>
            public void EventTotalDiceChange(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 || data.StageNum != 45) return;

                var setPlayer = data.Message[0] == "1" ? data.Player : data.Player.GetOppenentPlayer();

                playerDatas[(int)setPlayer].SC_EventTotalDiceChangeRecord[setPlayer.ToRelative(data.Player)][data.SkillType].Add(new()
                {
                    Type = data.Message[1] switch
                    {
                        "1" => EventTotalDiceChangeRecordType.Addition,
                        "2" => EventTotalDiceChangeRecordType.Subtraction,
                        "3" => EventTotalDiceChangeRecordType.Multiplication,
                        "4" => EventTotalDiceChangeRecordType.Division_Floor,
                        "5" => EventTotalDiceChangeRecordType.Division_Ceiling,
                        "6" => EventTotalDiceChangeRecordType.Assign,
                        _ => throw new NotImplementedException()
                    },
                    Value = Convert.ToInt32(data.Message[2])
                });
            }
        }
    }
}
