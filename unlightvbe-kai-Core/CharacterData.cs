using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    /// <summary>
    /// 角色資料層
    /// </summary>
    public class CharacterData
    {
        public Character Character { get; }
        public int CurrentHP { get; set; }
        public bool[] ActiveSkillIsActivate { get; set; }
        public bool[] PassiveSkillIsActivate { get; set; }
        public int[] ActiveSkillTurnOnCount { get; set; }
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
