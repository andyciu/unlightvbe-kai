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
            SampleData data = new();
            var player1 = data.GetPlayer(1);
            var player2 = data.GetPlayer(2);

            BattleSystem battleSystem = new(player1, player2);
            battleSystem.SetUserInterface(new ConsoleInterface("1", player1, player2),
                new AIWithConsoleInterface("2", player2, player1));

            Task.Run(() => { battleSystem.Start(); }).Wait();
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