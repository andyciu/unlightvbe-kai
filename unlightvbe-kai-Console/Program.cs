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
            var player1 = new SampleData().GetPlayer(1);
            var player2 = new SampleData().GetPlayer(2);

            var playerModel_p1 = GetPlayerModel(player1);
            var playerModel_p2 = GetPlayerModel(player2);
            var buffNameDict = SampleData.GetBuffNameDict();

            BattleSystem battleSystem = new(new BattleSystemInitialDataModel
            {
                Player1 = player1,
                Player2 = player2,
                UserInterface_P1 = new ConsoleInterface("1", playerModel_p1, playerModel_p2, buffNameDict),
                UserInterface_P2 = new AIWithConsoleInterface("2", playerModel_p2, playerModel_p1, buffNameDict),
                InitialCardList = SampleData.GetCardList_Deck(),
                BuffList = SampleData.GetBuffList()
            });

            Task.Run(battleSystem.Start).Wait();
        }

        private static ConsoleInterface.PlayerModel GetPlayerModel(Player player)
        {
            return new()
            {
                PlayerId = player.PlayerId,
                Name = player.Name,
                Characters = player.Deck.Deck_Subs.Select(x => new ConsoleInterface.CharacterModel
                {
                    Name = x.Character.Name,
                    Title = x.Character.Title,
                    HP = x.Character.HP,
                    ATK = x.Character.ATK,
                    DEF = x.Character.DEF,
                    VBEID = x.Character.VBEID,
                    EventColour = x.Character.EventColour,
                    LevelMain = x.Character.LevelMain,
                    LevelNum = x.Character.LevelNum,
                }).ToList()
            };
        }
        private static void test1()
        {
            List<Card> cards =
            [
                new ActionCard
                {
                    Number = 1,
                },
                new EventCard
                {
                    Number = 2,
                }
            ];

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
            var result = player.Deck.Deck_Subs[0].Character.ActiveSkills[0].Function.Invoke(new());
            Console.WriteLine(result?[0].Type.ToString());
        }
    }
}