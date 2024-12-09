using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models.Skill;

namespace unlightvbe_kai_core.Models
{
    public record BattleSystemInitialDataModel
    {
        public required Player Player1 { get; init; }
        public required Player Player2 { get; init; }
        public required IUserInterface UserInterface_P1 { get; init; }
        public required IUserInterface UserInterface_P2 { get; init; }
        public required List<ActionCard> InitialCardList { get; init; }
        public required List<BuffSkillModel> BuffList { get; init; }
    }
}
