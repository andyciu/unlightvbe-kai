namespace unlightvbe_kai_core.Models
{
    /// <summary>
    /// 角色資料層
    /// </summary>
    public class CharacterData(Character character)
    {
        public Character Character { get; } = character;
        public int CurrentHP { get; set; } = character.HP;
        /// <summary>
        /// 主動技能是否啟動標記
        /// </summary>
        public bool[] ActiveSkillIsActivate { get; set; } = new bool[character.ActiveSkills.Count];
        /// <summary>
        /// 被動技能是否啟動標記
        /// </summary>
        public bool[] PassiveSkillIsActivate { get; set; } = new bool[character.PassiveSkills.Count];
        /// <summary>
        /// 主動技能啟動次數
        /// </summary>
        public int[] ActiveSkillTurnOnCount { get; set; } = new int[character.ActiveSkills.Count];
        /// <summary>
        /// 被動技能啟動次數
        /// </summary>
        public int[] PassiveSkillTurnOnCount { get; set; } = new int[character.PassiveSkills.Count];
        /// <summary>
        /// 異常狀態列表
        /// </summary>
        public List<BuffData> BuffDatas { get; set; } = [];
    }
}
