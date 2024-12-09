using System.Diagnostics.CodeAnalysis;
using unlightvbe_kai_core.Models.Skill;

namespace unlightvbe_kai_core.Models
{
    public class Character
    {
        public required string Name { get; init; }
        public string Title { get; init; } = string.Empty;
        public required int HP { get; init; }
        public required int ATK { get; init; }
        public required int DEF { get; init; }
        public required string VBEID { get; init; }
        public required string EventColour { get; init; }
        public required string LevelMain { get; init; }
        public required int LevelNum { get; init; }
        public List<ActiveSkillModel> ActiveSkills { get; init; } = [];
        public List<PassiveSkillModel> PassiveSkills { get; init; } = [];
        public Character() { }
        [SetsRequiredMembers]
        public Character(Character character)
        {
            Name = character.Name;
            Title = character.Title;
            HP = character.HP;
            ATK = character.ATK;
            DEF = character.DEF;
            VBEID = character.VBEID;
            EventColour = character.EventColour;
            LevelMain = character.LevelMain;
            LevelNum = character.LevelNum;
            ActiveSkills = new(character.ActiveSkills);
            PassiveSkills = new(character.PassiveSkills);
        }
    }
}
