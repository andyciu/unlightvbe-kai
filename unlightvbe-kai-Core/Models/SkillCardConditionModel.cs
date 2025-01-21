using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillArgs;

namespace unlightvbe_kai_core.Models
{
    public class SkillCardConditionModel
    {
        public required SkillCardConditionScopeType Scope { get; set; }
        public required ActionCardType CardType { get; set; }
        public required int Number { get; set; }
    }
}
