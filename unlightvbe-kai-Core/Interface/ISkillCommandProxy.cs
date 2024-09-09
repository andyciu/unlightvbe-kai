using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core.Interface
{
    /// <summary>
    /// 技能執行指令解釋器介面
    /// </summary>
    public interface ISkillCommandProxy
    {
        public void ExecuteCommands(int stageNum, UserPlayerType player, SkillType skillType, List<SkillCommandModel> commandList);
    }
}
