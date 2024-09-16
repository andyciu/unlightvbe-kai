using unlightvbe_kai_core;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_Data;

namespace unlightvbe_kai_console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            //Test2();
            //return;

            //Sample Data
            SampleData data = new();
            var player1 = data.GetPlayer(1);
            var player2 = data.GetPlayer(2);

            BattleSystem battleSystem = new(player1, player2);
            battleSystem.SetUserInterface(new ConsoleInterface("1", player1, player2),
                new AIWithConsoleInterface("2", player2, player1));
            battleSystem.SetInitialCardDeck(SampleData.GetCardList_Deck());

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

        private static void Test2()
        {
            //Sample Data
            SampleData data = new();
            var player1 = data.GetPlayer(1);

            Test2_2(player1);
        }

        private static void Test2_2(Player player)
        {
            var result = player.Deck.Deck_Subs[0].character.ActiveSkills[0].Function.Invoke(new());
            Console.WriteLine(result?[0].Type.ToString());
        }
    }
}