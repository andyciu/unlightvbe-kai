using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_console
{
    public class AIWithConsoleInterface(string instanceName, Player selfPlayer, Player opponentPlayer) : ConsoleInterface(instanceName, selfPlayer, opponentPlayer)
    {
        public override ReadActionModel ReadAction()
        {
            if (PhaseType == PhaseType.Move)
            {
                Thread.Sleep(1000);
                return new ReadActionModel()
                {
                    Type = UserActionType.OKButtonClick
                };
            }
            else
            {
                return base.ReadAction();
            }
        }

        protected override void SetConsoleColor()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
    }
}
