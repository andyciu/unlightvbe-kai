using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public delegate List<SkillCommandModel> ActiveSkill(ActiveSkillArgsModel args);
    public delegate List<SkillCommandModel> PassiveSkill(PassiveSkillArgsModel args);
}
