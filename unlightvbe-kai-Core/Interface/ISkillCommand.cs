using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core.Interface
{
    /// <summary>
    /// 技能執行指令介面
    /// </summary>
    public interface ISkillCommand
    {
        public void SkillLineLight(SkillCommandDataModel data);
        public void SkillTurnOnOff(SkillCommandDataModel data);
        public void SkillLineLightAnother(SkillCommandDataModel data);
        public void SkillTurnOnOffAnother(SkillCommandDataModel data);
        public void SkillAnimateStartPlay(SkillCommandDataModel data);
        public void EventTotalDiceChange(SkillCommandDataModel data);
    }
}
