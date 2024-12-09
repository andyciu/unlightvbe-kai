using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.SkillArgs;

namespace unlightvbe_kai_core
{
    public delegate List<SkillCommandModel> ActiveSkillDelegate(ActiveSkillArgsModel args);
    public delegate List<SkillCommandModel> PassiveSkillDelegate(PassiveSkillArgsModel args);
    public delegate List<SkillCommandModel> BuffDelegate(BuffArgsModel args);
}
