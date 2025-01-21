using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.UserActionProxy;

namespace unlightvbe_kai_core.Interface
{
    public interface IUserAction
    {
        public UserActionCardClickType CardClick(UserPlayerType player, int cardNumber, PhaseType phaseType);
        public UserActionCardReverseLocation CardReverse(UserPlayerType player, int cardNumber, PhaseType phaseType);
        public bool BarMoveLeft(UserPlayerType player);
        public bool BarMoveRight(UserPlayerType player);
        public bool BarMoveStay(UserPlayerType player);
        public bool BarMoveChange(UserPlayerType player);
        public bool OKButtonClick(UserPlayerType player);
        public int GetPlayerDiceTotalNumber(UserPlayerType player);
        public bool ChangeCharacter(UserPlayerType player, string NewCharacterVBEID);
        public (bool, string) ChangeCharacterRandom(UserPlayerType player);
        public bool IsMustBeChangeCharacter(UserPlayerType player);
    }
}
