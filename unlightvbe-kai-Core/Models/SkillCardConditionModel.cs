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
