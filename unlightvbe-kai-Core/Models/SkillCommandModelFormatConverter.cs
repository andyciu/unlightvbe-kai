using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Interface;

namespace unlightvbe_kai_core.Models
{
    /// <summary>
    /// 執行指令格式轉換器類別
    /// </summary>
    public class SkillCommandModelFormatConverter : ISkillCommandModelFormatConverter
    {
        private List<SkillCommandModel> skillCommandModels = new();
        public SkillCommandModelFormatConverter() { }

        /// <summary>
        /// 匯出執行指令集合
        /// </summary>
        /// <returns></returns>
        public List<SkillCommandModel> Output()
        {
            return skillCommandModels;
        }

        /// <summary>
        /// 人物必殺技狀態燈控制(自身)
        /// </summary>
        /// <remarks>
        /// {Active/Passive}(!=45/!=99)
        /// </remarks>
        /// <param name="isTurnOn">是否開啟</param>
        public void SkillLineLight(bool isTurnOn)
        {
            skillCommandModels.Add(new(SkillCommandType.SkillLineLight, isTurnOn ? "1" : "2"));
        }

        /// <summary>
        /// 人物必殺技狀態燈控制(其他)
        /// </summary>
        /// <remarks>
        /// {Active/Passive}[Normal/Event]
        /// </remarks>
        /// <param name="skillType">技能體系</param>
        /// <param name="skillNum">技能順序(1~4)</param>
        /// <param name="isTurnOn">是否開啟</param>
        /// <exception cref="NotImplementedException">技能體系非主動技能/被動技能時</exception>
        public void SkillLineLightAnother(SkillType skillType, int skillNum, bool isTurnOn)
        {
            skillCommandModels.Add(new(SkillCommandType.SkillLineLightAnother, skillType switch
            {
                SkillType.ActiveSkill => "1",
                SkillType.PassiveSkill => "2",
                _ => throw new NotImplementedException(),
            }, skillNum.ToString(), isTurnOn ? "1" : "2"));
        }

        /// <summary>
        /// 人物必殺技啟動碼控制(自身)
        /// </summary>
        /// <remarks>
        /// {Active/Passive}(!=45/!=99)
        /// </remarks>
        /// <param name="isTurnOn">是否開啟</param>
        public void SkillTurnOnOff(bool isTurnOn)
        {
            skillCommandModels.Add(new(SkillCommandType.SkillTurnOnOff, isTurnOn ? "1" : "2"));
        }

        /// <summary>
        /// 人物必殺技啟動碼控制(其他)
        /// </summary>
        /// <remarks>
        /// {Active/Passive}[Normal/Event]
        /// </remarks>
        /// <param name="skillType">技能體系</param>
        /// <param name="skillNum">技能順序(1~4)</param>
        /// <param name="isTurnOn">是否開啟</param>
        /// <exception cref="NotImplementedException">技能體系非主動技能/被動技能時</exception>
        public void SkillTurnOnOffAnother(SkillType skillType, int skillNum, bool isTurnOn)
        {
            skillCommandModels.Add(new(SkillCommandType.SkillTurnOnOffAnother, skillType switch
            {
                SkillType.ActiveSkill => "1",
                SkillType.PassiveSkill => "2",
                _ => throw new NotImplementedException(),
            }, skillNum.ToString(), isTurnOn ? "1" : "2"));
        }

        /// <summary>
        /// 人物必殺技狀態燈控制+啟動碼控制(自身)
        /// </summary>
        /// <remarks>
        /// {Active/Passive}(!=45/!=99)
        /// </remarks>
        /// <param name="isTurnOn">是否開啟</param>
        public void SkillTurnOnOffWithLineLight(bool isTurnOn)
        {
            SkillLineLight(isTurnOn);
            SkillTurnOnOff(isTurnOn);
        }

        /// <summary>
        /// 技能動畫圖像執行
        /// </summary>
        /// <remarks>
        /// [Normal]{!=Buff}(!=13/!=33)<br/>
        /// ※觸發執行階段(61)事件
        /// </remarks>
        public void SkillAnimateStartPlay()
        {
            skillCommandModels.Add(new(SkillCommandType.SkillAnimateStartPlay));
        }

        /// <summary>
        /// 攻擊/防禦階段系統骰數變化量控制
        /// </summary>
        /// <remarks>
        /// (45)
        /// </remarks>
        /// <param name="player">目標玩家方</param>
        /// <param name="recordType">變化量控制種類</param>
        /// <param name="value">數值</param>
        public void EventTotalDiceChange(UserPlayerRelativeType player, NumberChangeRecordSixVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventTotalDiceChange, ((int)player + 1).ToString(), ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 攻擊/防禦階段角色白值能力對骰數變化量控制
        /// </summary>
        /// <remarks>
        /// (45)
        /// </remarks>
        /// <param name="player">目標玩家方</param>
        /// <param name="recordType">變化量控制種類</param>
        /// <param name="value">數值</param>
        public void EventPersonAbilityDiceChange(UserPlayerRelativeType player, NumberChangeRecordThreeVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventPersonAbilityDiceChange, ((int)player + 1).ToString(), ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 攻擊/防禦階段擲骰前系統總骰數直接控制
        /// </summary>
        /// <remarks>
        /// (10/11/30/31)
        /// </remarks>
        /// <param name="player">目標玩家方</param>
        /// <param name="recordType">變化量控制種類</param>
        /// <param name="value">數值</param>
        public void PersonTotalDiceControl(UserPlayerRelativeType player, NumberChangeRecordSixVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonTotalDiceControl, ((int)player + 1).ToString(), ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 擲骰後正面骰數控制
        /// </summary>
        /// <remarks>
        /// (20/21/22/23/24/25/26/27/28/29)
        /// </remarks>
        /// <param name="recordType">變化量控制種類</param>
        /// <param name="value">數值</param>
        public void AttackTrueDiceControl(NumberChangeRecordThreeVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.AttackTrueDiceControl, ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 系統回合數控制
        /// </summary>
        /// <remarks>
        /// [Normal/Event]
        /// </remarks>
        /// <param name="recordType">變化量控制種類</param>
        /// <param name="value">數值</param>
        public void BattleTurnControl(NumberChangeRecordTwoVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.BattleTurnControl, ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 發布訊息效果執行
        /// </summary>
        /// <param name="message">訊息內容</param>
        public void BattleSendMessage(string message)
        {
            skillCommandModels.Add(new(SkillCommandType.BattleSendMessage, message));
        }

        /// <summary>
        /// 擲骰效果執行
        /// </summary>
        /// <remarks>
        /// (13/33/20/21/22/23/24/25/26/27/28/29)<br/>
        /// ※(62)
        /// </remarks>
        public void BattleStartDice()
        {
            skillCommandModels.Add(new(SkillCommandType.BattleStartDice));
        }

        /// <summary>
        /// 角色相對距離控制
        /// </summary>
        /// <remarks>
        /// [Normal/Event](!=47)<br/>
        /// ※(47)
        /// </remarks>
        /// <param name="distanceType"></param>
        public void BattleMoveControl(PlayerDistanceType distanceType)
        {
            skillCommandModels.Add(new(SkillCommandType.BattleMoveControl, ((int)distanceType).ToString()));
        }

        /// <summary>
        /// 原應執行之距離變更無效化
        /// </summary>
        /// <remarks>
        /// (47)
        /// </remarks>
        public void EventMoveActionOff()
        {
            skillCommandModels.Add(new(SkillCommandType.EventMoveActionOff));
        }

        /// <summary>
        /// 人物移動階段總移動量控制
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <param name="recordType">變化量控制種類</param>
        /// <param name="value">數值</param>
        /// <remarks>
        /// (2/3/4/70)
        /// </remarks>
        public void PersonMoveControl(UserPlayerRelativeType player, NumberChangeRecordThreeVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonMoveControl, ((int)player + 1).ToString(), ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 人物角色移動階段行動控制
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <param name="recordType">行動控制種類</param>
        /// <remarks>
        /// (2/3/4/70)
        /// </remarks>
        public void PersonMoveActionChange(UserPlayerRelativeType player, PersonMoveActionType recordType)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonMoveActionChange, ((int)player + 1).ToString(), ((int)recordType).ToString()));
        }

        /// <summary>
        /// 人物角色優先攻擊控制
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <remarks>
        /// (2/3/4/70/71)
        /// </remarks>
        public void PersonAttackFirstControl(UserPlayerRelativeType player)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonAttackFirstControl, ((int)player + 1).ToString()));
        }

        /// <summary>
        /// 人物角色血量控制
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <param name="characterNum">目標方角色順序(台上1>台下2/3)</param>
        /// <param name="controlType">控制種類</param>
        /// <param name="value">數值</param>
        /// <remarks>
        /// [Normal/Event](!=46/!=48)<br/>
        /// ※(46[DirectDamage/Death])<br/>
        /// ※(48[Heal])
        /// </remarks>
        public void PersonBloodControl(UserPlayerRelativeType player, int characterNum, PersonBloodControlType controlType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonBloodControl, ((int)player + 1).ToString(), characterNum.ToString(), ((int)controlType).ToString(), value.ToString()));
        }
    }
}
