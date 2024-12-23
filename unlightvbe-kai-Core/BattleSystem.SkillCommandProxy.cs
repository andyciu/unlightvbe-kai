using System.Diagnostics;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Enum.StageMessage;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public partial class BattleSystem
    {
        /// <summary>
        /// 技能執行指令解釋器類別
        /// </summary>
        protected class SkillCommandProxyClass(BattleSystem battleSystem) : ISkillCommand, ISkillCommandProxy
        {
            private PlayerData[] playerDatas = battleSystem.PlayerDatas;
            /// /// <summary>
            /// 執行階段階層計數參考
            /// </summary>
            private int SkillStageCallCount => battleSystem.SkillAdapter.StageCallCount;
            /// <summary>
            /// 人物角色血量控制觸發事件(46)反映紀錄
            /// </summary>
            /// <remarks>
            /// Record: <br/>
            /// <list type="number">
            ///     <item>
            ///         <term>EventBloodActionOff</term>
            ///         <description>[MainProperty]bool(0)</description>
            ///     </item>
            ///     <item>
            ///         <term>EventBloodActionChange</term>
            ///         <description>[MainProperty]bool(1)/Record</description>
            ///     </item>
            /// </list>
            /// </remarks>
            public Dictionary<int, PropertyWithRecord<(bool, bool), (NumberChangeRecordThreeVersionType, int)>> EventBloodActionRecord { get; set; } = [];
            /// <summary>
            /// 人物角色血量控制觸發事件(48)反映紀錄
            /// </summary>
            /// <remarks>
            /// Record: <br/>
            /// <list type="number">
            ///     <item>
            ///         <term>EventHealActionOff</term>
            ///         <description>[MainProperty]bool(0)</description>
            ///     </item>
            ///     <item>
            ///         <term>EventHealActionChange</term>
            ///         <description>[MainProperty]bool(1)/Record</description>
            ///     </item>
            /// </list>
            /// </remarks>
            public Dictionary<int, PropertyWithRecord<(bool, bool), (NumberChangeRecordThreeVersionType, int)>> EventHealActionRecord { get; set; } = [];
            /// <summary>
            /// 異常狀態消滅時觸發事件(73)反映紀錄
            /// </summary>
            /// <remarks>
            /// EventRemoveBuffActionOff - bool
            /// </remarks>
            public Dictionary<int, bool> EventRemoveBuffActionRecord { get; set; } = [];
            /// <summary>
            /// 執行指令呼叫執行
            /// </summary>
            /// <param name="commandList">指令集合</param>
            public void ExecuteCommands(SkillCommandProxyExecueCommandDataModel data, List<SkillCommandModel> commandList)
            {
                foreach (SkillCommandModel command in commandList)
                {
                    if (!CheckIsAuthorize(data, command.Type)) continue;

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
                        method.Invoke(this, [methodData]);
                    }
                }
            }

            /// <summary>
            /// 檢查是否符合技能發動驗證
            /// </summary>
            /// <param name="data"></param>
            /// <param name="commandType"></param>
            /// <returns></returns>
            private bool CheckIsAuthorize(SkillCommandProxyExecueCommandDataModel data, SkillCommandType commandType)
            {
                switch (data.SkillType)
                {
                    case SkillType.ActiveSkill:
                        if (playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex].ActiveSkillIsActivate[data.SkillIndex] ||
                            CheckCommandIsAllowOnUnAuthMode(commandType))
                        {
                            return true;
                        }
                        break;
                    case SkillType.PassiveSkill:
                        if (playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex].PassiveSkillIsActivate[data.SkillIndex] ||
                            CheckCommandIsAllowOnUnAuthMode(commandType))
                        {
                            return true;
                        }
                        break;
                    case SkillType.Buff:
                    case SkillType.CharacterActualStatus:
                        return true;
                }
                return false;
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
                    battleSystem.MultiUIAdapter.ActiveSkillLineLight(data.Player, data.SkillIndex,
                    data.Message[0] == "1");
                }
                else if (data.SkillType == SkillType.PassiveSkill)
                {
                    battleSystem.MultiUIAdapter.UpdateDataRelative(
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
                        battleSystem.MultiUIAdapter.ActiveSkillLineLight(data.Player, Convert.ToInt32(data.Message[1]) - 1,
                            data.Message[2] == "1");
                        break;
                    case "2": //PassiveSkill
                        battleSystem.MultiUIAdapter.UpdateDataRelative(
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

                battleSystem.MultiUIAdapter.ShowSkillAnimate(data.Player, data.SkillID);

                battleSystem.SkillAdapter.StageStartSkillOnly_Active_Passive(61, data.Player, data.CharacterBattleIndex, data.SkillType, data.SkillIndex, null);
            }

            /// <summary>
            /// 攻擊/防禦階段系統骰數變化量控制
            /// </summary>
            /// <param name="data"></param>
            public void EventTotalDiceChange(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 || data.StageNum != 45)
                { CommandExportException(); return; }

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

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

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

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
                    !(new int[] { 10, 11, 12, 30, 31, 32 }).Any(x => x == data.StageNum))
                { CommandExportException(); return; }

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

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
                        battleSystem.DiceTrueTotal += Convert.ToInt32(data.Message[1]);
                        break;
                    case NumberChangeRecordThreeVersionType.Subtraction:
                        battleSystem.DiceTrueTotal -= Convert.ToInt32(data.Message[1]);
                        break;
                    case NumberChangeRecordThreeVersionType.Assign:
                        battleSystem.DiceTrueTotal = Convert.ToInt32(data.Message[1]);
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
                        battleSystem.TurnNum += Convert.ToInt32(data.Message[1]);
                        break;
                    case NumberChangeRecordTwoVersionType.Subtraction:
                        battleSystem.TurnNum -= Convert.ToInt32(data.Message[1]);
                        break;
                }

                if (battleSystem.TurnNum < 1) battleSystem.TurnNum = 1;

                battleSystem.MultiUIAdapter.UpdateData_All(new()
                {
                    Type = UpdateDataType.TurnNumber,
                    Value = battleSystem.TurnNum
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

                battleSystem.MultiUIAdapter.ShowBattleMessage(data.Message[0]);
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
                    tmpDiceTrue[i] = DiceAction(playerDatas[i].DiceTotal);
                }
                //正面骰數計算
                UserPlayerType attackPlyaer, defensePlayer;
                attackPlyaer = battleSystem.Phase[(int)UserPlayerType.Player1] == PhaseType.Attack ? UserPlayerType.Player1 : UserPlayerType.Player2;
                defensePlayer = attackPlyaer.GetOppenentPlayer();

                var tmpDiceTrueTotal = tmpDiceTrue[(int)attackPlyaer] - tmpDiceTrue[(int)defensePlayer];

                battleSystem.SkillAdapter.StageStartSkillOnly_Active_Passive(62, data.Player, data.CharacterBattleIndex, data.SkillType, data.SkillIndex,
                    [
                        playerDatas[(int)UserPlayerType.Player1].DiceTotal.ToString(),
                        playerDatas[(int)UserPlayerType.Player2].DiceTotal.ToString(),
                        tmpDiceTrue[(int)UserPlayerType.Player1].ToString(),
                        tmpDiceTrue[(int)UserPlayerType.Player2].ToString(),
                        tmpDiceTrueTotal.ToString()
                    ]);

                battleSystem.MultiUIAdapter.UIShowDice(playerDatas[(int)UserPlayerType.Player1].DiceTotal, playerDatas[(int)UserPlayerType.Player2].DiceTotal,
                    tmpDiceTrue[(int)UserPlayerType.Player1], tmpDiceTrue[(int)UserPlayerType.Player2],
                    attackPlyaer == UserPlayerType.Player1 ? DiceType.Attack : DiceType.Defense,
                    attackPlyaer == UserPlayerType.Player2 ? DiceType.Attack : DiceType.Defense);
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

                var newDistance = (CommandPlayerDistanceType)Convert.ToInt32(data.Message[0]);

                battleSystem.ChangePlayerDistance(newDistance.ToPlayerDistanceType(), true, data.Player.ToTriggerPlayerType());
            }

            /// <summary>
            /// 原應執行之距離變更無效化
            /// </summary>
            /// <param name="data"></param>
            public void EventMoveActionOff(SkillCommandDataModel data)
            {
                if (data.StageNum != 47)
                { CommandExportException(); return; }

                battleSystem.m_playerDistance.RecordValue = true;
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

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

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

                if (selectType == PersonMoveActionType.BarMoveChange && battleSystem.PlayerVersusMode != PlayerVersusModeType.ThreeOnThree)
                { CommandExportException(); return; }

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

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


                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

                battleSystem.m_AttackPhaseFirst.MainProperty = setPlayer;
                battleSystem.m_AttackPhaseFirst.RecordValue = true;
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

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();
                var setType = (PersonBloodControlType)Convert.ToInt32(data.Message[2]);

                int characterNum = Convert.ToInt32(data.Message[1]);
                if (characterNum <= 0 || characterNum > playerDatas[(int)setPlayer].CharacterDatas.Count)
                { CommandExportException(); return; }

                switch (setType)
                {
                    case PersonBloodControlType.DirectDamage:
                    case PersonBloodControlType.Death:
                        battleSystem.CharacterHPDamage(new()
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
                        battleSystem.CharacterHPHeal(new()
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

            /// <summary>
            /// 原應執行之傷害無效化
            /// </summary>
            /// <param name="data"></param>
            public void EventBloodActionOff(SkillCommandDataModel data)
            {
                if (data.StageNum != 46)
                { CommandExportException(); return; }

                this.EventBloodActionRecord[SkillStageCallCount].MainProperty = (true, this.EventBloodActionRecord[SkillStageCallCount].MainProperty.Item2);
            }

            /// <summary>
            /// 原應執行之傷害效果變更
            /// </summary>
            /// <param name="data"></param>
            public void EventBloodActionChange(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 2 || data.StageNum != 46)
                { CommandExportException(); return; }

                var setType = (NumberChangeRecordThreeVersionType)Convert.ToInt32(data.Message[0]);
                this.EventBloodActionRecord[SkillStageCallCount].MainProperty = (this.EventBloodActionRecord[SkillStageCallCount].MainProperty.Item1, true);
                this.EventBloodActionRecord[SkillStageCallCount].RecordValue = (setType, Convert.ToInt32(data.Message[1]));
            }

            /// <summary>
            /// 傷害反射效果執行
            /// </summary>
            /// <param name="data"></param>
            public void EventBloodReflection(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 ||
                    (data.StageNum != 46 && data.StageNum != 48))
                { CommandExportException(); return; }

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

                int characterNum = Convert.ToInt32(data.Message[1]);
                if (characterNum <= 0 || characterNum > playerDatas[(int)setPlayer].CharacterDatas.Count)
                { CommandExportException(); return; }

                battleSystem.CharacterHPDamage(new()
                {
                    Player = setPlayer,
                    CharacterVBEID = playerDatas[(int)setPlayer].CharacterDatas[characterNum - 1].Character.VBEID,
                    DamageNumber = Convert.ToInt32(data.Message[2]),
                    DamageType = CharacterHPDamageType.Direct,
                    IsCallEvent = false,
                    TriggerPlayerType = data.Player.ToTriggerPlayerType(),
                    TriggerSkillType = data.SkillType.ToTriggerSkillType()
                });
            }

            /// <summary>
            /// 原應執行之回復無效化
            /// </summary>
            /// <param name="data"></param>
            public void EventHealActionOff(SkillCommandDataModel data)
            {
                if (data.StageNum != 48)
                { CommandExportException(); return; }

                this.EventHealActionRecord[SkillStageCallCount].MainProperty = (true, this.EventHealActionRecord[SkillStageCallCount].MainProperty.Item2);
            }

            /// <summary>
            /// 原應執行之回復效果變更
            /// </summary>
            /// <param name="data"></param>
            public void EventHealActionChange(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 2 || data.StageNum != 48)
                { CommandExportException(); return; }

                var setType = (NumberChangeRecordThreeVersionType)Convert.ToInt32(data.Message[0]);
                this.EventHealActionRecord[SkillStageCallCount].MainProperty = (this.EventHealActionRecord[SkillStageCallCount].MainProperty.Item1, true);
                this.EventHealActionRecord[SkillStageCallCount].RecordValue = (setType, Convert.ToInt32(data.Message[1]));
            }

            /// <summary>
            /// 回復反射效果執行
            /// </summary>
            /// <param name="data"></param>
            public void EventHealReflection(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 3 ||
                    (data.StageNum != 46 && data.StageNum != 48))
                { CommandExportException(); return; }

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

                int characterNum = Convert.ToInt32(data.Message[1]);
                if (characterNum <= 0 || characterNum > playerDatas[(int)setPlayer].CharacterDatas.Count)
                { CommandExportException(); return; }

                battleSystem.CharacterHPHeal(new()
                {
                    Player = setPlayer,
                    CharacterVBEID = playerDatas[(int)setPlayer].CharacterDatas[characterNum - 1].Character.VBEID,
                    HealNumber = Convert.ToInt32(data.Message[2]),
                    IsCallEvent = false,
                    TriggerPlayerType = data.Player.ToTriggerPlayerType(),
                    TriggerSkillType = data.SkillType.ToTriggerSkillType()
                });
            }

            /// <summary>
            /// 人物角色新增異常狀態
            /// </summary>
            /// <param name="data"></param>
            public void PersonAddBuff(SkillCommandDataModel data)
            {
                if (data.Message == null || data.Message.Length != 5 || data.SkillType == SkillType.Buff ||
                    (data.StageType != SkillStageType.Normal && data.StageType != SkillStageType.Event))
                { CommandExportException(); return; }

                var playerType = (CommandPlayerRelativeTwoVersionType)Convert.ToInt32(data.Message[0]);
                var setPlayer = playerType == CommandPlayerRelativeTwoVersionType.Self ? data.Player : data.Player.GetOppenentPlayer();

                int characterNum = Convert.ToInt32(data.Message[1]);
                if (characterNum <= 0 || characterNum > playerDatas[(int)setPlayer].CharacterDatas.Count)
                { CommandExportException(); return; }

                if (battleSystem.BuffList.Any(x => x.Identifier == data.Message[2]))
                {
                    var tmpBuffData = playerDatas[(int)setPlayer].CharacterDatas[characterNum - 1].BuffDatas.Find(x => x.Buff.Identifier == data.Message[2]);
                    if (tmpBuffData != null)
                    {
                        tmpBuffData.Value = Convert.ToInt32(data.Message[3]);
                        tmpBuffData.Total = Convert.ToInt32(data.Message[4]);
                    }
                    else
                    {
                        tmpBuffData = new BuffData
                        {
                            Buff = battleSystem.BuffList.First(x => x.Identifier == data.Message[2]),
                            Value = Convert.ToInt32(data.Message[3]),
                            Total = Convert.ToInt32(data.Message[4])
                        };
                        playerDatas[(int)setPlayer].CharacterDatas[characterNum - 1].BuffDatas.Add(tmpBuffData);
                    }


                    battleSystem.MultiUIAdapter.BuffDataSet(setPlayer, playerDatas[(int)setPlayer].CharacterDatas[characterNum - 1].Character.VBEID,
                        new()
                        {
                            Identifier = tmpBuffData.Buff.Identifier,
                            Value = tmpBuffData.Value,
                            Total = tmpBuffData.Total,
                        });

                    //執行階段(72)-異常狀態新增時
                    battleSystem.SkillAdapter.StageStartSkillOnly_Buff(72, setPlayer, characterNum - 1, tmpBuffData.Buff.Identifier, null);

                    //執行階段(76)-人物角色附加狀態增加時
                    battleSystem.SkillAdapter.StageStart(76, setPlayer, true, true,
                        [
                            ((int)setPlayer).ToString(),
                            ((int)SkillType.Buff).ToString(),
                            tmpBuffData.Buff.Identifier
                        ]);
                }
            }

            /// <summary>
            /// 異常狀態宣告當回合結束
            /// </summary>
            /// <param name="data"></param>
            public void BuffTurnEnd(SkillCommandDataModel data)
            {
                if (data.SkillType != SkillType.Buff || (data.StageType != SkillStageType.Normal && data.StageType != SkillStageType.Event) ||
                    (data.StageNum == 72 || data.StageNum == 73))
                { CommandExportException(); return; }

                var buffData = playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex]
                    .BuffDatas.Where(x => x.Buff.Identifier == data.SkillID).First();

                buffData.Total -= 1;

                if (buffData.Total <= 0)
                {
                    //執行階段(73)-異常狀態消滅時
                    battleSystem.SkillAdapter.StageStartSkillOnly_Buff(73, data.Player, data.CharacterBattleIndex, buffData.Buff.Identifier,
                        [
                            ((int)StageMessage73_RemoveType.Active).ToString()
                        ]);

                    playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex].BuffDatas.Remove(buffData);

                    battleSystem.MultiUIAdapter.BuffDataRemove(data.Player, playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex].Character.VBEID, buffData.Buff.Identifier);

                    //執行階段(77)-人物角色附加狀態解除時
                    battleSystem.SkillAdapter.StageStart(77, data.Player, true, true,
                        [
                            ((int)data.Player).ToString(),
                            ((int)SkillType.Buff).ToString(),
                            buffData.Buff.Identifier
                        ]);
                }
            }

            /// <summary>
            /// 異常狀態宣告結束
            /// </summary>
            /// <param name="data"></param>
            public void BuffEnd(SkillCommandDataModel data)
            {
                if (data.SkillType != SkillType.Buff || (data.StageType != SkillStageType.Normal && data.StageType != SkillStageType.Event) ||
                    (data.StageNum == 72 || data.StageNum == 73))
                { CommandExportException(); return; }

                var buffData = playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex]
                    .BuffDatas.Where(x => x.Buff.Identifier == data.SkillID).First();

                //執行階段(73)-異常狀態消滅時
                battleSystem.SkillAdapter.StageStartSkillOnly_Buff(73, data.Player, data.CharacterBattleIndex, buffData.Buff.Identifier,
                    [
                        ((int)StageMessage73_RemoveType.Active).ToString()
                    ]);

                playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex].BuffDatas.Remove(buffData);

                battleSystem.MultiUIAdapter.BuffDataRemove(data.Player, playerDatas[(int)data.Player].CharacterDatas[data.CharacterBattleIndex].Character.VBEID, buffData.Buff.Identifier);

                //執行階段(77)-人物角色附加狀態解除時
                battleSystem.SkillAdapter.StageStart(77, data.Player, true, true,
                    [
                        ((int)data.Player).ToString(),
                        ((int)SkillType.Buff).ToString(),
                        buffData.Buff.Identifier
                    ]);
            }

            /// <summary>
            /// 原應執行之異常狀態消除無效化
            /// </summary>
            /// <param name="data"></param>
            public void EventRemoveBuffActionOff(SkillCommandDataModel data)
            {
                if (data.StageNum != 73 || data.SkillType != SkillType.Buff)
                { CommandExportException(); return; }

                if (EventRemoveBuffActionRecord.ContainsKey(SkillStageCallCount))
                {
                    this.EventRemoveBuffActionRecord[SkillStageCallCount] = true;
                }
                else
                {
                    CommandExportException(); //是否為主動觸發應於各狀態內判斷
                }
            }
        }
    }
}
