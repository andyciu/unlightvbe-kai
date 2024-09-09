using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Models
{
    public class SkillCardConditionModel
    {
        public SkillCardConditionScopeType Scope { get; set; }
        public ActionCardType CardType { get; set; }
        public int Number { get; set; }
    }
}
