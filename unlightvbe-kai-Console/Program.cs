using unlightvbe_kai_core;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //Sample Data
            Character character1 = new()
            {
                Name = "OO",
                HP = 1,
                ATK = 2,
                DEF = 3,
                VBEID = "RR1",
                EventColour = "000000",
                LevelMain = "LV",
                LevelNum = 1,
            };

            Character character2 = new()
            {
                Name = "PP",
                HP = 3,
                ATK = 1,
                DEF = 2,
                VBEID = "PP1",
                EventColour = "000000",
                LevelMain = "LV",
                LevelNum = 1,
            };

            Deck deck1 = new()
            {
                Name = "D1",
                Deck_Subs = new List<Deck_Sub>
                {
                    new()
                    {
                        character = character1,
                        eventCards = SampleData.GetCardList_Event()
                    }
                }
            };

            Deck deck2 = new()
            {
                Name = "D2",
                Deck_Subs = new List<Deck_Sub>
                {
                    new()
                    {
                        character = character2,
                        eventCards = SampleData.GetCardList_Event()
                    }
                }
            };

            Player player1 = new()
            {
                Name = "AA",
                PlayerId = 1,
                Decks = new List<Deck> { deck1 }
            };
            Player player2 = new()
            {
                Name = "BB",
                PlayerId = 2,
                Decks = new List<Deck> { deck2 }
            };

            BattleSystem battleSystem = new(player1, player2, 0, 0);
            battleSystem.SetUserInterface(new ConsoleInterface("1"), new AIWithConsoleInterface("2"));

            Task.Run(() =>
            {
                battleSystem.Start();
            }).Wait();
        }
        private static void test1()
        {
            List<Card> cards = new List<Card>
            {
                new ActionCard
                {
                    Number = 1,
                },
                new EventCard
                {
                    Number= 2,
                }
            };

            foreach (var card in cards)
            {
                switch (card)
                {
                    case ActionCard:
                        Console.WriteLine("AAACCC");
                        break;
                    case EventCard:
                        Console.WriteLine(card.GetType());
                        break;
                }
            }
        }
    }
}