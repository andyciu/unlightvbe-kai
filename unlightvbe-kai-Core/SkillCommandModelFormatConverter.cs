using unlightvbe_kai_core.Enum.Skill;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    /// <summary>
    /// 執行指令格式轉換器類別
    /// </summary>
    public class SkillCommandModelFormatConverter : ISkillCommandModelFormatConverter
    {
        private readonly List<SkillCommandModel> skillCommandModels = [];
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
        /// ※(61)
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
        public void EventTotalDiceChange(CommandPlayerRelativeTwoVersionType player, NumberChangeRecordSixVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventTotalDiceChange, ((int)player).ToString(), ((int)recordType).ToString(), value.ToString()));
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
        public void EventPersonAbilityDiceChange(CommandPlayerRelativeTwoVersionType player, NumberChangeRecordThreeVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventPersonAbilityDiceChange, ((int)player).ToString(), ((int)recordType).ToString(), value.ToString()));
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
        public void PersonTotalDiceControl(CommandPlayerRelativeTwoVersionType player, NumberChangeRecordSixVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonTotalDiceControl, ((int)player).ToString(), ((int)recordType).ToString(), value.ToString()));
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
        public void BattleMoveControl(CommandPlayerDistanceType distanceType)
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
        public void PersonMoveControl(CommandPlayerRelativeTwoVersionType player, NumberChangeRecordThreeVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonMoveControl, ((int)player).ToString(), ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 人物角色移動階段行動控制
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <param name="recordType">行動控制種類</param>
        /// <remarks>
        /// (2/3/4/70)
        /// </remarks>
        public void PersonMoveActionChange(CommandPlayerRelativeTwoVersionType player, PersonMoveActionType recordType)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonMoveActionChange, ((int)player).ToString(), ((int)recordType).ToString()));
        }

        /// <summary>
        /// 人物角色優先攻擊控制
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <remarks>
        /// (2/3/4/70/71)
        /// </remarks>
        public void PersonAttackFirstControl(CommandPlayerRelativeTwoVersionType player)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonAttackFirstControl, ((int)player).ToString()));
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
        public void PersonBloodControl(CommandPlayerRelativeTwoVersionType player, int characterNum, PersonBloodControlType controlType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonBloodControl, ((int)player).ToString(), characterNum.ToString(), ((int)controlType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 原應執行之傷害無效化
        /// </summary>
        /// <remarks>
        /// (46)
        /// </remarks>
        public void EventBloodActionOff()
        {
            skillCommandModels.Add(new(SkillCommandType.EventBloodActionOff));
        }

        /// <summary>
        /// 原應執行之傷害效果變更
        /// </summary>
        /// <param name="recordType">控制種類</param>
        /// <param name="value">數值</param>
        /// <remarks>
        /// (46)
        /// </remarks>
        public void EventBloodActionChange(NumberChangeRecordThreeVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventBloodActionChange, ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 傷害反射效果執行
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <param name="characterNum">目標方角色順序(台上1>台下2/3)</param>
        /// <param name="value">數值</param>
        /// <remarks>
        /// (46/48)
        /// </remarks>
        public void EventBloodReflection(CommandPlayerRelativeTwoVersionType player, int characterNum, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventBloodReflection, ((int)player).ToString(), characterNum.ToString(), value.ToString()));
        }

        /// <summary>
        /// 原應執行之回復無效化
        /// </summary>
        /// <remarks>
        /// (48)
        /// </remarks>
        public void EventHealActionOff()
        {
            skillCommandModels.Add(new(SkillCommandType.EventHealActionOff));
        }

        /// <summary>
        /// 原應執行之回復效果變更
        /// </summary>
        /// <param name="recordType">控制種類</param>
        /// <param name="value">數值</param>
        /// <remarks>
        /// (48)
        /// </remarks>
        public void EventHealActionChange(NumberChangeRecordThreeVersionType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventHealActionChange, ((int)recordType).ToString(), value.ToString()));
        }

        /// <summary>
        /// 回復反射效果執行
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <param name="characterNum">目標方角色順序(台上1>台下2/3)</param>
        /// <param name="value">數值</param>
        /// <remarks>
        /// (46/48)
        /// </remarks>
        public void EventHealReflection(CommandPlayerRelativeTwoVersionType player, int characterNum, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventHealReflection, ((int)player).ToString(), characterNum.ToString(), value.ToString()));
        }

        /// <summary>
        /// 人物角色新增異常狀態
        /// </summary>
        /// <param name="player">目標玩家方</param>
        /// <param name="characterNum">目標方角色順序(台上1>台下2/3)</param>
        /// <param name="buffIdentifier">異常狀態之技能唯一識別碼</param>
        /// <param name="buffValue">效果變化量</param>
        /// <param name="buffTotal">效果回合數</param>
        /// <remarks>
        /// [Normal/Event]{!Buff}<br/>
        /// ※(72[異常狀態新增時]/76[人物角色附加狀態增加時])
        /// </remarks>
        public void PersonAddBuff(CommandPlayerRelativeTwoVersionType player, int characterNum, string buffIdentifier, int buffValue, int buffTotal)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonAddBuff, ((int)player).ToString(), characterNum.ToString(), buffIdentifier, buffValue.ToString(), buffTotal.ToString()));
        }

        /// <summary>
        /// 異常狀態宣告當回合結束
        /// </summary>
        /// <remarks>
        /// {Buff}[Normal/Event](!=72/!=73)<br/>
        /// ※(73[異常狀態消滅時]/77*[人物角色附加狀態解除時])
        /// </remarks>
        public void BuffTurnEnd()
        {
            skillCommandModels.Add(new(SkillCommandType.BuffTurnEnd));
        }

        /// <summary>
        /// 異常狀態宣告結束
        /// </summary>
        /// <remarks>
        /// {Buff}[Normal/Event](!=72/!=73)<br/>
        /// ※(73[異常狀態消滅時]/77*[人物角色附加狀態解除時])
        /// </remarks>
        public void BuffEnd()
        {
            skillCommandModels.Add(new(SkillCommandType.BuffEnd));
        }

        /// <summary>
        /// 原應執行之異常狀態消除無效化
        /// </summary>
        /// <remarks>
        /// {Buff}(73)
        /// </remarks>
        public void EventRemoveBuffActionOff()
        {
            skillCommandModels.Add(new(SkillCommandType.EventRemoveBuffActionOff));
        }

        /// <summary>
        /// 人物角色移除異常狀態(全部)
        /// </summary>
        /// <remarks>
        /// [Normal/Event]{!Buff}<br/>
        /// ※(73[異常狀態消滅時]/77*[人物角色附加狀態解除時])
        /// </remarks>
        /// <param name="player">目標玩家方</param>
        /// <param name="characterNum">目標方角色順序(台上1>台下2/3)</param>
        public void PersonRemoveBuffAll(CommandPlayerRelativeTwoVersionType player, int characterNum)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonRemoveBuffAll, ((int)player).ToString(), characterNum.ToString()));
        }

        /// <summary>
        /// 人物角色移除異常狀態(指定)
        /// </summary>
        /// <remarks>
        /// [Normal/Event]{!Buff}<br/>
        /// ※(73[異常狀態消滅時]/77*[人物角色附加狀態解除時])
        /// </remarks>
        /// <param name="player">目標玩家方</param>
        /// <param name="characterNum">目標方角色順序(台上1>台下2/3)</param>
        /// <param name="buffIdentifier">異常狀態之技能唯一識別碼</param>
        public void PersonRemoveBuffSelect(CommandPlayerRelativeTwoVersionType player, int characterNum, string buffIdentifier)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonRemoveBuffSelect, ((int)player).ToString(), characterNum.ToString(), buffIdentifier));
        }

        /// <summary>
        /// 人物角色異常狀態變更回合數
        /// </summary>
        /// <remarks>
        /// [Normal/Event]{!Buff}<br/>
        /// ※(73[異常狀態消滅時]/77*[人物角色附加狀態解除時])
        /// </remarks>
        /// <param name="player">目標玩家方</param>
        /// <param name="characterNum">目標方角色順序(台上1>台下2/3)</param>
        /// <param name="buffIdentifier">異常狀態之技能唯一識別碼</param>
        /// <param name="changeType">控制種類</param>
        /// <param name="changeValue">數值</param>
        public void PersonBuffTurnChange(CommandPlayerRelativeTwoVersionType player, int characterNum, string buffIdentifier, NumberChangeRecordThreeVersionType changeType, int changeValue)
        {
            skillCommandModels.Add(new(SkillCommandType.PersonBuffTurnChange, ((int)player).ToString(), characterNum.ToString(), buffIdentifier, ((int)changeType).ToString(), changeValue.ToString()));
        }
    }
}
