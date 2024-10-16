using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models
{
    public class CharacterHPHealDataModel
    {
        public UserPlayerType Player { get; set; }
        public string CharacterVBEID { get; set; }
        public int HealNumber { get; set; }
        public bool IsCallEvent { get; set; }
        public CommandPlayerType TriggerPlayerType { get; set; }
        public TriggerSkillType TriggerSkillType { get; set; }
    }
}
