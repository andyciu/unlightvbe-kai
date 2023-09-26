using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_console
{
    public class AIWithConsoleInterface : ConsoleInterface
    {
        public AIWithConsoleInterface(string instanceName, Player selfPlayer, Player opponentPlayer) :
            base(instanceName, selfPlayer, opponentPlayer)
        { }

        public override ReadActionModel ReadAction()
        {
            if (PhaseStartType == PhaseStartType.Move)
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
