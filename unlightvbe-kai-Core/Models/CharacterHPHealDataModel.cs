using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models
{
    public class CharacterHPHealDataModel
    {
        public required UserPlayerType Player { get; set; }
        public required string CharacterVBEID { get; set; }
        public required int HealNumber { get; set; }
        public required bool IsCallEvent { get; set; }
        public required CommandPlayerType TriggerPlayerType { get; set; }
        public required TriggerSkillType TriggerSkillType { get; set; }
    }
}
