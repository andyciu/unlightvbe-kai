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
        public void EventTotalDiceChange(UserPlayerRelativeType player, NumberChangeRecordSixVersionType recordType, int value);
        public void EventPersonAbilityDiceChange(UserPlayerRelativeType player, NumberChangeRecordThreeVersionType recordType, int value);
        public void PersonTotalDiceControl(UserPlayerRelativeType player, NumberChangeRecordSixVersionType recordType, int value);
        public void AttackTrueDiceControl(NumberChangeRecordThreeVersionType recordType, int value);
        public void BattleStartDice();
        public void BattleMoveControl(PlayerDistanceType distanceType);
        public void EventMoveActionOff();
        public void PersonMoveControl(UserPlayerRelativeType player, NumberChangeRecordThreeVersionType recordType, int value);
        public void PersonMoveActionChange(UserPlayerRelativeType player, PersonMoveActionType recordType);
        public void PersonAttackFirstControl(UserPlayerRelativeType player);
        public void PersonBloodControl(UserPlayerRelativeType player, int characterNum, PersonBloodControlType controlType, int value);
    }
}
