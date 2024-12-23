using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Interface
{
    /// <summary>
    /// 執行指令格式轉換器介面
    /// </summary>
    public interface ISkillCommandModelFormatConverter
    {
        public void SkillLineLight(bool isTurnOn);
        public void SkillTurnOnOff(bool isTurnOn);
        public void SkillTurnOnOffWithLineLight(bool isTurnOn);
        public void SkillLineLightAnother(SkillType skillType, int skillNum, bool isTurnOn);
        public void SkillTurnOnOffAnother(SkillType skillType, int skillNum, bool isTurnOn);
        public void SkillAnimateStartPlay();
        public void BattleTurnControl(NumberChangeRecordTwoVersionType recordType, int value);
        public void BattleSendMessage(string message);
        public void EventTotalDiceChange(CommandPlayerRelativeTwoVersionType player, NumberChangeRecordSixVersionType recordType, int value);
        public void EventPersonAbilityDiceChange(CommandPlayerRelativeTwoVersionType player, NumberChangeRecordThreeVersionType recordType, int value);
        public void PersonTotalDiceControl(CommandPlayerRelativeTwoVersionType player, NumberChangeRecordSixVersionType recordType, int value);
        public void AttackTrueDiceControl(NumberChangeRecordThreeVersionType recordType, int value);
        public void BattleStartDice();
        public void BattleMoveControl(CommandPlayerDistanceType distanceType);
        public void EventMoveActionOff();
        public void PersonMoveControl(CommandPlayerRelativeTwoVersionType player, NumberChangeRecordThreeVersionType recordType, int value);
        public void PersonMoveActionChange(CommandPlayerRelativeTwoVersionType player, PersonMoveActionType recordType);
        public void PersonAttackFirstControl(CommandPlayerRelativeTwoVersionType player);
        public void PersonBloodControl(CommandPlayerRelativeTwoVersionType player, int characterNum, PersonBloodControlType controlType, int value);
        public void EventBloodActionOff();
        public void EventBloodActionChange(NumberChangeRecordThreeVersionType recordType, int value);
        public void EventBloodReflection(CommandPlayerRelativeTwoVersionType player, int characterNum, int value);
        public void EventHealActionOff();
        public void EventHealActionChange(NumberChangeRecordThreeVersionType recordType, int value);
        public void EventHealReflection(CommandPlayerRelativeTwoVersionType player, int characterNum, int value);
        public void PersonAddBuff(CommandPlayerRelativeTwoVersionType player, int characterNum, string buffIdentifier, int buffValue, int buffTotal);
        public void BuffTurnEnd();
        public void BuffEnd();
        public void EventRemoveBuffActionOff();
    }
}
