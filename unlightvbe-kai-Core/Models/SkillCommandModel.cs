using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models
{
    public class SkillCommandModel
    {
        public SkillCommandType Type { get; set; }
        public string[]? Message { get; set; }

        public SkillCommandModel(SkillCommandType type, params string[]? message)
        {
            Type = type;
            Message = message;
        }
    }
}
