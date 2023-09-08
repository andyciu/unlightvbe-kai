using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_console
{
    internal class AIWithConsoleInterface : ConsoleInterface
    {
        public AIWithConsoleInterface(string instanceName) : base(instanceName) { }

        public override ReadActionModel ReadAction()
        {
            Thread.Sleep(1000);
            return new ReadActionModel()
            {
                Type = UserActionType.OKButtonClick
            };
        }

        internal override void SetConsoleColor()
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
    }
}
