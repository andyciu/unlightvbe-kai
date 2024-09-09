using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core.Interface
{
    /// <summary>
    /// 技能執行指令介面
    /// </summary>
    public interface ISkillCommand
    {
        public void AtkingLineLight(SkillCommandDataModel data);
    }
}
