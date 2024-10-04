using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;

namespace unlightvbe_kai_core.Models
{
    public class CharacterHPDamageDataModel
    {
        public UserPlayerType Player { get; set; }
        public string CharacterVBEID { get; set; }
        public int DamageNumber { get; set; }
        public CharacterHPDamageType DamageType { get; set; }
        public bool IsCallEvent { get; set; }
        public TriggerPlayerType TriggerPlayerType { get; set; }
        public TriggerSkillType TriggerSkillType { get; set; }
    }
}
