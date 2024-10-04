using System.Diagnostics;
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

            private static void CommandExportException()
            {
                if (Debugger.IsAttached)
                {
                    throw new Exception();
                }
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
                { CommandExportException(); return; }


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
                { CommandExportException(); return; }

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
                { CommandExportException(); return; }

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
                { CommandExportException(); return; }

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
            /// <remarks>
            /// ※觸發執行階段(61)事件
            /// </remarks>
            /// <param name="data"></param>
            public void SkillAnimateStartPlay(SkillCommandDataModel data)
            {
                if (data.StageType != SkillStageType.Normal || data.SkillType == SkillType.Buff ||
                    data.StageNum == 13 || data.StageNum == 33)
                { CommandExportException(); return; }

                BattleSystem.MultiUIAdapter.ShowSkillAnimate(data.Player, data.SkillID);

                BattleSystem.SkillAdapter.StageStartSkillOnly(61, data.Player, data.CharacterBattleIndex, data.SkillType, data.SkillIndex);
            }

            /// <summary>
            /// 攻擊/防禦階段系統骰數變化量控制
            /// </summary>
            /// <param name="data"></param>
            public void EventTotalDiceChange(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 || data.StageNum != 45)
                { CommandExportException(); return; }

                var setPlayer = data.Message[0] == "1" ? data.Player : data.Player.GetOppenentPlayer();

                playerDatas[(int)setPlayer].SC_EventTotalDiceChangeRecord.MainProperty[setPlayer.ToRelative(data.Player)][data.SkillType].Add(new()
                {
                    Type = (NumberChangeRecordSixVersionType)Convert.ToInt32(data.Message[1]),
                    Value = Convert.ToInt32(data.Message[2])
                });
            }

            /// <summary>
            /// 攻擊/防禦階段角色白值能力對骰數變化量控制
            /// </summary>
            /// <param name="data"></param>
            public void EventPersonAbilityDiceChange(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 || data.StageNum != 45)
                { CommandExportException(); return; }

                var setPlayer = data.Message[0] == "1" ? data.Player : data.Player.GetOppenentPlayer();

                playerDatas[(int)setPlayer].SC_EventPersonAbilityDiceChangeRecord.MainProperty[setPlayer.ToRelative(data.Player)][data.SkillType].Add(new()
                {
                    Type = (NumberChangeRecordThreeVersionType)Convert.ToInt32(data.Message[1]),
                    Value = Convert.ToInt32(data.Message[2])
                });
            }

            /// <summary>
            /// 攻擊/防禦階段擲骰前系統總骰數直接控制
            /// </summary>
            /// <param name="data"></param>
            public void PersonTotalDiceControl(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 ||
                    !(new int[] { 10, 11, 30, 31 }).Any(x => x == data.StageNum))
                { CommandExportException(); return; }

                var setPlayer = data.Message[0] == "1" ? data.Player : data.Player.GetOppenentPlayer();
                switch ((NumberChangeRecordSixVersionType)Convert.ToInt32(data.Message[1]))
                {
                    case NumberChangeRecordSixVersionType.Addition:
                        playerDatas[(int)setPlayer].DiceTotal += Convert.ToInt32(data.Message[2]);
                        break;
                    case NumberChangeRecordSixVersionType.Subtraction:
                        playerDatas[(int)setPlayer].DiceTotal -= Convert.ToInt32(data.Message[2]);
                        break;
                    case NumberChangeRecordSixVersionType.Multiplication:
                        playerDatas[(int)setPlayer].DiceTotal *= Convert.ToInt32(data.Message[2]);
                        break;
                    case NumberChangeRecordSixVersionType.Division_Floor:
                        playerDatas[(int)setPlayer].DiceTotal = (int)Math.Floor((double)playerDatas[(int)setPlayer].DiceTotal / Convert.ToInt32(data.Message[2]));
                        break;
                    case NumberChangeRecordSixVersionType.Division_Ceiling:
                        playerDatas[(int)setPlayer].DiceTotal = (int)Math.Ceiling((double)playerDatas[(int)setPlayer].DiceTotal / Convert.ToInt32(data.Message[2]));
                        break;
                    case NumberChangeRecordSixVersionType.Assign:
                        playerDatas[(int)setPlayer].DiceTotal = Convert.ToInt32(data.Message[2]);
                        break;
                }
            }

            /// <summary>
            /// 擲骰後正面骰數控制
            /// </summary>
            /// <param name="data"></param>
            public void AttackTrueDiceControl(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 2 ||
                    !(new int[] { 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 }).Any(x => x == data.StageNum))
                { CommandExportException(); return; }

                switch ((NumberChangeRecordThreeVersionType)Convert.ToInt32(data.Message[0]))
                {
                    case NumberChangeRecordThreeVersionType.Addition:
                        BattleSystem.DiceTrueTotal += Convert.ToInt32(data.Message[1]);
                        break;
                    case NumberChangeRecordThreeVersionType.Subtraction:
                        BattleSystem.DiceTrueTotal -= Convert.ToInt32(data.Message[1]);
                        break;
                    case NumberChangeRecordThreeVersionType.Assign:
                        BattleSystem.DiceTrueTotal = Convert.ToInt32(data.Message[1]);
                        break;
                }
            }

            /// <summary>
            /// 系統回合數控制
            /// </summary>
            /// <param name="data"></param>
            public void BattleTurnControl(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 2 ||
                    (data.StageType != SkillStageType.Normal && data.StageType != SkillStageType.Event))
                { CommandExportException(); return; }

                switch ((NumberChangeRecordTwoVersionType)Convert.ToInt32(data.Message[0]))
                {
                    case NumberChangeRecordTwoVersionType.Addition:
                        BattleSystem.TurnNum += Convert.ToInt32(data.Message[1]);
                        break;
                    case NumberChangeRecordTwoVersionType.Subtraction:
                        BattleSystem.TurnNum -= Convert.ToInt32(data.Message[1]);
                        break;
                }

                BattleSystem.MultiUIAdapter.UpdateData_All(new()
                {
                    Type = UpdateDataType.TurnNumber,
                    Value = BattleSystem.TurnNum
                });
            }

            /// <summary>
            /// 發布訊息效果執行
            /// </summary>
            /// <param name="data"></param>
            public void BattleSendMessage(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 1 || data.StageNum == 99)
                { CommandExportException(); return; }

                BattleSystem.MultiUIAdapter.ShowBattleMessage(data.Message[0]);
            }

            /// <summary>
            /// 擲骰效果執行
            /// </summary>
            /// <remarks>
            /// ※觸發執行階段(62)事件
            /// </remarks>
            /// <param name="data"></param>
            public void BattleStartDice(SkillCommandDataModel data)
            {
                if (!(new int[] { 13, 33, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29 }).Any(x => x == data.StageNum))
                { CommandExportException(); return; }

                //擲骰
                var tmpDiceTrue = new int[2];
                for (int i = 0; i < playerDatas.Length; i++)
                {
                    tmpDiceTrue[i] = BattleSystem.DiceAction(playerDatas[i].DiceTotal);
                }
                //傷害計算
                UserPlayerType attackPlyaer, defensePlayer;
                attackPlyaer = BattleSystem.Phase[(int)UserPlayerType.Player1] == PhaseType.Attack ? UserPlayerType.Player1 : UserPlayerType.Player2;
                defensePlayer = attackPlyaer.GetOppenentPlayer();

                var tmpDiceTrueTotal = tmpDiceTrue[(int)attackPlyaer] - tmpDiceTrue[(int)defensePlayer];

                skillAdapter.StageStartSkillOnly(62, data.Player, data.CharacterBattleIndex, data.SkillType, data.SkillIndex,
                    new string[] {
                        playerDatas[(int)UserPlayerType.Player1].DiceTotal.ToString(),
                        playerDatas[(int)UserPlayerType.Player2].DiceTotal.ToString(),
                        tmpDiceTrue[(int)UserPlayerType.Player1].ToString(),
                        tmpDiceTrue[(int)UserPlayerType.Player2].ToString(),
                        tmpDiceTrueTotal.ToString()
                    });
            }

            /// <summary>
            /// 角色相對距離控制
            /// </summary>
            /// <remarks>
            /// ※觸發執行階段(47)事件
            /// </remarks>
            /// <param name="data"></param>
            public void BattleMoveControl(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 1 || data.StageNum == 47 ||
                    (data.StageType != SkillStageType.Normal && data.StageType != SkillStageType.Event))
                { CommandExportException(); return; }

                var newDistance = (PlayerDistanceType)Convert.ToInt32(data.Message[0]);

                BattleSystem.ChangePlayerDistance(newDistance, true, data.Player.ToTriggerPlayerType());
            }

            /// <summary>
            /// 原應執行之距離變更無效化
            /// </summary>
            /// <param name="data"></param>
            public void EventMoveActionOff(SkillCommandDataModel data)
            {
                if (data.StageNum != 47)
                { CommandExportException(); return; }

                BattleSystem.m_playerDistance.RecordValue = true;
            }

            /// <summary>
            /// 人物移動階段總移動量控制
            /// </summary>
            /// <param name="data"></param>
            public void PersonMoveControl(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 ||
                    !(new int[] { 2, 3, 4, 70 }).Any(x => x == data.StageNum))
                { CommandExportException(); return; }

                var setPlayer = data.Message[0] == "1" ? data.Player : data.Player.GetOppenentPlayer();

                playerDatas[(int)setPlayer].SC_PersonMoveControlRecord[setPlayer.ToRelative(data.Player)].Add(new()
                {
                    Type = (NumberChangeRecordThreeVersionType)Convert.ToInt32(data.Message[1]),
                    Value = Convert.ToInt32(data.Message[2])
                });
            }

            /// <summary>
            /// 人物角色移動階段行動控制
            /// </summary>
            /// <param name="data"></param>
            public void PersonMoveActionChange(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 2 ||
                    !(new int[] { 2, 3, 4, 70 }).Any(x => x == data.StageNum))
                { CommandExportException(); return; }

                var selectType = (PersonMoveActionType)Convert.ToInt32(data.Message[1]);

                if (selectType == PersonMoveActionType.BarMoveChange && BattleSystem.PlayerVersusMode != PlayerVersusModeType.ThreeOnThree)
                { CommandExportException(); return; }

                var setPlayer = data.Message[0] == "1" ? data.Player : data.Player.GetOppenentPlayer();

                playerDatas[(int)setPlayer].MoveBarSelect = selectType.ToMoveBarSelectType();
            }

            /// <summary>
            /// 人物角色優先攻擊控制
            /// </summary>
            /// <param name="data"></param>
            public void PersonAttackFirstControl(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 1 ||
                    !(new int[] { 2, 3, 4, 70, 71 }).Any(x => x == data.StageNum))
                { CommandExportException(); return; }


                var setPlayer = data.Message[0] == "1" ? data.Player : data.Player.GetOppenentPlayer();

                BattleSystem.m_AttackPhaseFirst.MainProperty = setPlayer;
                BattleSystem.m_AttackPhaseFirst.RecordValue = true;
            }

            /// <summary>
            /// 人物角色血量控制
            /// </summary>
            /// <param name="data"></param>
            public void PersonBloodControl(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 4 || data.StageNum == 46 || data.StageNum == 48 ||
                    (data.StageType != SkillStageType.Normal && data.StageType != SkillStageType.Event))
                { CommandExportException(); return; }

                var setPlayer = data.Message[0] == "1" ? data.Player : data.Player.GetOppenentPlayer();
                var setType = (PersonBloodControlType)Convert.ToInt32(data.Message[2]);

                int characterNum = Convert.ToInt32(data.Message[1]);
                if (characterNum <= 0 || characterNum > playerDatas[(int)setPlayer].CharacterDatas.Count)
                { CommandExportException(); return; }

                switch (setType)
                {
                    case PersonBloodControlType.DirectDamage:
                    case PersonBloodControlType.Death:
                        BattleSystem.CharacterHPDamage(new()
                        {
                            Player = setPlayer,
                            CharacterVBEID = playerDatas[(int)setPlayer].CharacterDatas[characterNum - 1].Character.VBEID,
                            DamageNumber = Convert.ToInt32(data.Message[3]),
                            DamageType = setType == PersonBloodControlType.DirectDamage ? CharacterHPDamageType.Direct : CharacterHPDamageType.Death,
                            IsCallEvent = true,
                            TriggerPlayerType = data.Player.ToTriggerPlayerType(),
                            TriggerSkillType = data.SkillType.ToTriggerSkillType()
                        });
                        break;
                    case PersonBloodControlType.Heal:
                        BattleSystem.CharacterHPHeal(new()
                        {
                            Player = setPlayer,
                            CharacterVBEID = playerDatas[(int)setPlayer].CharacterDatas[characterNum - 1].Character.VBEID,
                            HealNumber = Convert.ToInt32(data.Message[3]),
                            IsCallEvent = true,
                            TriggerPlayerType = data.Player.ToTriggerPlayerType(),
                            TriggerSkillType = data.SkillType.ToTriggerSkillType()
                        });
                        break;
                }
            }
        }
    }
}
