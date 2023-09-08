using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public class PlayerData
    {
        public Player Player { get; set; }
        public int DeckIndex { get; set; }
        public int HoldMaxCount { get; set; }
        public IUserInterfaceAsync UserInterface { get; set; }
        public MoveBarSelectType MoveBarSelect { get; set; }
        public bool IsOKButtonSelect { get; set; }
        public PlayerData(Player player, int deckIndex)
        {
            Player = player;
            DeckIndex = deckIndex;
        }
    }
}
