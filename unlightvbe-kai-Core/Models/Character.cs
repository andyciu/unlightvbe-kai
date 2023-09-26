using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace unlightvbe_kai_core.Models
{
    public class Character
    {
        public string Name { get; set; }
        public string? Title { get; set; }
        public int HP { get; set; }
        public int ATK { get; set; }
        public int DEF { get; set; }
        public string VBEID { get; set; }
        public string EventColour { get; set; }
        public string LevelMain { get; set; }
        public int LevelNum { get; set; }

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
        }
    }
}
