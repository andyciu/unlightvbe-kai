namespace unlightvbe_kai_core.Models
{
    public class Character
    {
        public string Name { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public int HP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public string VBEID { get; set; } = string.Empty;
        public string EventColour { get; set; } = string.Empty;
        public string LevelMain { get; set; } = string.Empty;
        public int LevelNum { get; set; }
        public List<Skill<ActiveSkill>> ActiveSkills { get; set; } = new();
        public List<Skill<PassiveSkill>> PassiveSkills { get; set; } = new();
        public Character() { }
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
