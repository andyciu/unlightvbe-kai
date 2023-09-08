using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;

namespace unlightvbe_kai_core.Interface
{
    public interface IUserAction
    {
        public UserActionCardClickType CardClick(UserPlayerType player, int cardNumber);
        public UserActionCardReverseLocation CardReverse(UserPlayerType player, int cardNumber);
        public bool BarMoveLeft(UserPlayerType player);
        public bool BarMoveRight(UserPlayerType player);
        public bool BarMoveStay(UserPlayerType player);
        public bool BarMoveChange(UserPlayerType player);
        public bool OKButtonClick(UserPlayerType player);
    }
}
