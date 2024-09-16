using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models
{
    public class SkillCommandDataModel : SkillCommandProxyExecueCommandDataModel
    {
        /// <summary>
        /// 指令引數
        /// </summary>
        public string[]? Message { get; set; }
    }
}
