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
        public void EventTotalDiceChange(UserPlayerRelativeType player, EventTotalDiceChangeRecordType recordType, int value);
    }
}
