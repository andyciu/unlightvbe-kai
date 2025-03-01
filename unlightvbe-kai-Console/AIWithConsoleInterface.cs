﻿using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.UserActionProxy;
using unlightvbe_kai_core.Models.UserInterface;
using static unlightvbe_kai_console.ConsoleInterface;

namespace unlightvbe_kai_console
{
    public class AIWithConsoleInterface(string instanceName, PlayerModel selfPlayer, PlayerModel opponentPlayer, Dictionary<string, string> buffNameDict) : ConsoleInterface(instanceName, selfPlayer, opponentPlayer, buffNameDict)
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

        protected override string ShowInstanceName()
        {
            return "\t\t\t\t\t\t[" + InstanceName + "]";
        }
    }
}
