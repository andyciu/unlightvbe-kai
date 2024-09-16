using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models.SkillCommand
{
    /// <summary>
    /// 執行指令-攻擊/防禦階段系統骰數變化量控制紀錄紀載
    /// </summary>
    public class EventTotalDiceChangeRecordModel
    {
        public EventTotalDiceChangeRecordType Type { get; set; }
        public int Value { get; set; }
    }
}
