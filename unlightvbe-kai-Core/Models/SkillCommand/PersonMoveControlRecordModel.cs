using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.SkillCommand
{
    /// <summary>
    /// 執行指令-人物移動階段總移動量控制紀錄紀載
    /// </summary>
    public class PersonMoveControlRecordModel
    {
        public NumberChangeRecordThreeVersionType Type { get; set; }
        public int Value { get; set; }
    }
}
