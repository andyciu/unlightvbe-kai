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

        public List<SkillCommandModel> Output()
        {
            return skillCommandModels;
        }

        public void SkillLineLight(bool isTurnOn)
        {
            skillCommandModels.Add(new(SkillCommandType.SkillLineLight, isTurnOn ? "1" : "2"));
        }

        public void SkillLineLightAnother(SkillType skillType, int skillNum, bool isTurnOn)
        {
            skillCommandModels.Add(new(SkillCommandType.SkillLineLightAnother, skillType switch
            {
                SkillType.ActiveSkill => "1",
                SkillType.PassiveSkill => "2",
                _ => throw new NotImplementedException(),
            }, skillNum.ToString(), isTurnOn ? "1" : "2"));
        }

        public void SkillTurnOnOff(bool isTurnOn)
        {
            skillCommandModels.Add(new(SkillCommandType.SkillTurnOnOff, isTurnOn ? "1" : "2"));
        }

        public void SkillTurnOnOffAnother(SkillType skillType, int skillNum, bool isTurnOn)
        {
            skillCommandModels.Add(new(SkillCommandType.SkillTurnOnOffAnother, skillType switch
            {
                SkillType.ActiveSkill => "1",
                SkillType.PassiveSkill => "2",
                _ => throw new NotImplementedException(),
            }, skillNum.ToString(), isTurnOn ? "1" : "2"));
        }

        public void SkillTurnOnOffWithLineLight(bool isTurnOn)
        {
            SkillLineLight(isTurnOn);
            SkillTurnOnOff(isTurnOn);
        }

        public void SkillAnimateStartPlay()
        {
            skillCommandModels.Add(new(SkillCommandType.SkillAnimateStartPlay));
        }

        public void EventTotalDiceChange(UserPlayerRelativeType player, EventTotalDiceChangeRecordType recordType, int value)
        {
            skillCommandModels.Add(new(SkillCommandType.EventTotalDiceChange, ((int)player + 1).ToString(), ((int)recordType).ToString(), value.ToString()));
        }
    }
}
