namespace unlightvbe_kai_core.Models
{
    /// <summary>
    /// 角色資料層
    /// </summary>
    public class CharacterData
    {
        public Character Character { get; }
        public int CurrentHP { get; set; }
        /// <summary>
        /// 主動技能是否啟動標記
        /// </summary>
        public bool[] ActiveSkillIsActivate { get; set; }
        /// <summary>
        /// 被動技能是否啟動標記
        /// </summary>
        public bool[] PassiveSkillIsActivate { get; set; }
        /// <summary>
        /// 主動技能啟動次數
        /// </summary>
        public int[] ActiveSkillTurnOnCount { get; set; }
        /// <summary>
        /// 被動技能啟動次數
        /// </summary>
        public int[] PassiveSkillTurnOnCount { get; set; }
        public CharacterData(Character character)
        {
            Character = character;
            CurrentHP = character.HP;
            ActiveSkillIsActivate = new bool[character.ActiveSkills.Count];
            PassiveSkillIsActivate = new bool[character.PassiveSkills.Count];
            ActiveSkillTurnOnCount = new int[character.ActiveSkills.Count];
            PassiveSkillTurnOnCount = new int[character.PassiveSkills.Count];
        }
    }
}
