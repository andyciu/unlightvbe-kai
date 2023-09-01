using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Interface;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.IUserInterface;

namespace unlightvbe_kai_core
{
    public class MultiUserInterfaceAdapter
    {
        internal IUserInterfaceAsync UserInterface_P1 { get; set; }
        internal IUserInterfaceAsync UserInterface_P2 { get; set; }

        public MultiUserInterfaceAdapter(IUserInterfaceAsync userInterface_P1, IUserInterfaceAsync userInterface_P2)
        {
            UserInterface_P1 = userInterface_P1;
            UserInterface_P2 = userInterface_P2;
        }

        public void ShowStartScreen(ShowStartScreenModel data)
        {
            Task task1 = Task.Run(() => { UserInterface_P1.ShowStartScreenAsync(data); });
            Task task2 = Task.Run(() =>
            {
                UserInterface_P2.ShowStartScreenAsync(new()
                {
                    Player1_Id = data.Player2_Id,
                    Player2_Id = data.Player1_Id,
                    Player1_DeckIndex = data.Player2_DeckIndex,
                    Player2_DeckIndex = data.Player1_DeckIndex,
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void ShowBattleMessage(string message)
        {
            Task task1 = Task.Run(() => { UserInterface_P1.ShowBattleMessageAsync(message); });
            Task task2 = Task.Run(() => { UserInterface_P2.ShowBattleMessageAsync(message); });
            Task.WhenAll(task1, task2).Wait();
        }

        public void DrawActionCard(List<Card> cards_p1, List<Card> cards_p2)
        {
            Task task1 = Task.Run(() =>
            {
                UserInterface_P1.DrawActionCardAsync(new()
                {
                    SelfCards = cards_p1.Select(card =>
                    {
                        return new CardModel
                        {
                            UpperType = card.UpperType,
                            UpperNum = card.UpperNum,
                            LowerType = card.LowerType,
                            LowerNum = card.LowerNum,
                            Location = card.Location,
                            Owner = card.Owner,
                            Number = card.Number,
                        };
                    }).ToList(),
                    OpponentCardCount = cards_p2.Count,
                });
            });
            Task task2 = Task.Run(() =>
            {
                UserInterface_P2.DrawActionCardAsync(new()
                {
                    SelfCards = cards_p2.Select(card =>
                    {
                        return new CardModel
                        {
                            UpperType = card.UpperType,
                            UpperNum = card.UpperNum,
                            LowerType = card.LowerType,
                            LowerNum = card.LowerNum,
                            Location = card.Location,
                            Owner = card.Owner,
                            Number = card.Number,
                        };
                    }).ToList(),
                    OpponentCardCount = cards_p1.Count,
                });
            });
            Task.WhenAll(task1, task2).Wait();
        }

        public void UpdateData(UpdateDataModel data)
        {
            Task task1 = Task.Run(() => { UserInterface_P1.UpdateData(data); });
            Task task2 = Task.Run(() => { UserInterface_P2.UpdateData(data); });
            Task.WhenAll(task1, task2).Wait();
        }
    }
}
