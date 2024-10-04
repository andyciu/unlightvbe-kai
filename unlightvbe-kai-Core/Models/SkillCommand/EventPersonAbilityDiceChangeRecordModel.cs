using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.SkillCommand
{
    /// <summary>
    /// 執行指令-攻擊/防禦階段角色白值能力對骰數變化量控制紀錄紀載
    /// </summary>
    public class EventPersonAbilityDiceChangeRecordModel
    {
        public NumberChangeRecordThreeVersionType Type { get; set; }
        public int Value { get; set; }
    }
}
