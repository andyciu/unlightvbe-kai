using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models
{
    public class SkillCommandDataModel
    {
        public int StageNum { get; set; }
        public UserPlayerType Player { get; set; }
        public SkillType SkillType { get; set; }
        public string[]? Message { get; set; }
    }
}
