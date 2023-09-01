using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_console
{
    internal class ConsoleInterface : IUserInterfaceAsync
    {
        public string InstanceName { get; set; }
        private List<CardModel> HoldCards { get; set; } = new List<CardModel>();
        private List<CardModel> PlayCards { get; set; } = new List<CardModel>();
        private int OpponentHoldCardCount;
        private int OpponentPlayCardCount;
        private int TurnNum;
        private int DeckNum;
        public ConsoleInterface(string instanceName)
        {
            InstanceName = instanceName;
        }

        private string showInstanceName()
        {
            return "[" + InstanceName + "]";
        }
        public void ShowStartScreen(ShowStartScreenModel data)
        {
            Console.WriteLine(showInstanceName() + "Test_ShowStartScreen");
            Console.WriteLine(showInstanceName() + "Player1_Id=" + data.Player1_Id);
            Console.WriteLine(showInstanceName() + "Player1_DeckIndex=" + data.Player1_DeckIndex);

            Thread.Sleep(1000);

            Console.WriteLine(showInstanceName() + "Player2_Id=" + data.Player2_Id);
            Console.WriteLine(showInstanceName() + "Player2_DeckIndex=" + data.Player2_DeckIndex);
        }

        public void ShowBattleMessage(string message)
        {
            Console.WriteLine(showInstanceName() + "Test_ShowBattleMessage");
            Console.WriteLine(showInstanceName() + message);
        }
        public ReadActionModel ReadAction()
        {
            string tmpstr = string.Empty;

            while (string.IsNullOrEmpty(tmpstr) || tmpstr.Split(' ').Length != 2)
            {
                Console.Write(showInstanceName() + "Action(\"Type Message\"): ");
                tmpstr = Console.ReadLine();
            }

            var tmparr = tmpstr.Split(' ');
            ReadActionModel readActionModel = new ReadActionModel();

            switch (tmparr[0])
            {
                case "CC":
                    readActionModel.Type = ReadActionType.CardClick;
                    readActionModel.Message = tmparr[1];
                    break;
                case "CR":
                    readActionModel.Type = ReadActionType.CardReverse;
                    readActionModel.Message = tmparr[1];
                    break;
                case "BARL":
                    readActionModel.Type = ReadActionType.BarMoveLeft;
                    break;
                case "BARR":
                    readActionModel.Type = ReadActionType.BarMoveRight;
                    break;
                case "BARS":
                    readActionModel.Type = ReadActionType.BarMoveStay;
                    break;
                case "BARC":
                    readActionModel.Type = ReadActionType.BarMoveChange;
                    break;
                case "OK":
                default:
                    readActionModel.Type = ReadActionType.OKButtonClick;
                    break;
            }

            return readActionModel;
        }
        public Task ShowStartScreenAsync(ShowStartScreenModel data)
        {
            ShowStartScreen(data);
            return Task.CompletedTask;
        }

        public Task ShowBattleMessageAsync(string message)
        {
            ShowBattleMessage(message);
            return Task.CompletedTask;
        }

        public Task DrawActionCardAsync(DrawActionCardModel data)
        {
            DrawActionCard(data);
            return Task.CompletedTask;
        }

        public void DrawActionCard(DrawActionCardModel data)
        {
            Console.WriteLine(showInstanceName() + "Test_DrawActionCard");

            foreach (var card in data.SelfCards)
            {
                HoldCards.Add(card);
                Console.WriteLine(showInstanceName() + "({0}){1}/{2}/{3}/{4}", card.Number, card.UpperType.ToString(), card.UpperNum, card.LowerType.ToString(), card.LowerNum);
            }

            OpponentHoldCardCount += data.OpponentCardCount;
            Console.WriteLine(showInstanceName() + "OpponentHoldCardCount=" + OpponentHoldCardCount);
        }

        public Task UpdateDataAsync(UpdateDataModel data)
        {
            UpdateDataAsync(data);
            return Task.CompletedTask;
        }

        public void UpdateData(UpdateDataModel data)
        {
            Console.WriteLine(showInstanceName() + "Test_UpdateData");
            Console.WriteLine(showInstanceName() + data.Type.ToString() + ":" + data.Value);

            switch (data.Type)
            {
                case UpdateDataType.TurnNumber:
                    TurnNum = data.Value;
                    break;
                case UpdateDataType.DeckNumber:
                    DeckNum = data.Value;
                    break;
            }
        }
    }
}
