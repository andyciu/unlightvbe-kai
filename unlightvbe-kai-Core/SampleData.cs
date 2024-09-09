using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_core
{
    public class SampleData
    {
        public List<Character> Characters { get; set; } = new();
        public List<Deck_Sub> Deck_Subs { get; set; } = new();
        public List<Deck> Decks { get; set; } = new();
        public List<Player> Players { get; set; } = new();
        public static readonly Random Rnd = new Random();
        public SampleData()
        {
            Skill<ActiveSkill> skill1 = new(TmpActiveSkill, "VBEID001")
            {
                Distance = new()
                        {
                            PlayerDistanceType.Middle,
                            PlayerDistanceType.Close,
                            PlayerDistanceType.Long,
                        },
                Phase = PhaseType.Attack,
                StageNumber = new() { 1, 2, 3 },
                Cards = new List<SkillCardConditionModel> { new()
                {
                    Scope = SkillCardConditionScopeType.Above,
                    CardType = ActionCardType.ATK_Sword,
                    Number = 1
                } },
                Name = "TmpActiveSkill"
            };

            Characters.Add(new()
            {
                Name = "Ria",
                HP = 9,
                ATK = 5,
                DEF = 10,
                VBEID = "S00105",
                EventColour = "357000",
                LevelMain = "LV",
                LevelNum = 5,
                ActiveSkills = new() { skill1 }
            });
            Characters.Add(new()
            {
                Name = "Evarist",
                HP = 12,
                ATK = 7,
                DEF = 7,
                VBEID = "N01105",
                EventColour = "325000",
                LevelMain = "LV",
                LevelNum = 5,
                ActiveSkills = new() { skill1 }
            });

            for (int i = 0; i < 2; i++)
            {
                Deck_Subs.Add(new()
                {
                    character = Characters[i],
                    eventCards = GetCardList_Event()
                });

                Decks.Add(new()
                {
                    Name = "D" + i.ToString(),
                    Deck_Subs = new() { Deck_Subs[i] }
                });
            }

            Players.Add(new()
            {
                Name = "AAA",
                PlayerId = 1,
                Deck = Decks[0]
            });

            Players.Add(new()
            {
                Name = "BBB",
                PlayerId = 2,
                Deck = Decks[1]
            });
        }
        public static List<ActionCard> GetCardList_Deck()
        {
            var cards = new List<ActionCard>();

            for (int i = 1; i <= 6; i++)
            {
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.ATK_Gun,
                    LowerNum = Rnd.Next(1, 7),
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.DEF,
                    LowerNum = Rnd.Next(1, 7),
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Gun,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.DEF,
                    LowerNum = Rnd.Next(1, 7),
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.SPE,
                    LowerNum = Rnd.Next(1, 7),
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Gun,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.SPE,
                    LowerNum = Rnd.Next(1, 7),
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.MOV,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.SPE,
                    LowerNum = Rnd.Next(1, 7),
                });
            }

            return cards;
        }

        public static List<EventCard> GetCardList_Event()
        {
            var cards = new List<EventCard>();

            for (int j = 1; j <= 6; j++)
            {
                cards.Add(new EventCard
                {
                    UpperType = (ActionCardType)Rnd.Next(1, 6),
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = (ActionCardType)Rnd.Next(1, 6),
                    LowerNum = Rnd.Next(1, 7),
                });
            }

            return cards;
        }

        public Player GetPlayer(int id)
        {
            return Players.Find(x => x.PlayerId == id) ?? Players[0];
        }

        public Character GetCharacter(string VBEID)
        {
            return Characters.Find(x => x.VBEID == VBEID) ?? Characters[0];
        }

        public List<SkillCommandModel> TmpActiveSkill(ActiveSkillArgsModel args)
        {
            return new()
            {
                new SkillCommandModel()
                {
                    Type = SkillCommandType.AtkingLineLight,
                    Message = new string[2] { "1", "2" }
                }
            };
        }
    }
}
