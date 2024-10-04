using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_Data.Character;

namespace unlightvbe_kai_Data
{
    public class SampleData
    {
        public List<unlightvbe_kai_core.Models.Character> Characters { get; set; } = new();
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
                Phase = PhaseType.Move,
                StageNumber = new() { 44, 2, 61, 46 },
                Cards = new List<SkillCardConditionModel> { new()
                {
                    Scope = SkillCardConditionScopeType.Above,
                    CardType = ActionCardType.None,
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

            //Characters.Add(Ria.GetCharacter());
            //Characters.Add(Evarist.GetCharacter());

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

        public unlightvbe_kai_core.Models.Character GetCharacter(string VBEID)
        {
            return Characters.Find(x => x.VBEID == VBEID) ?? Characters[0];
        }

        public List<SkillCommandModel> TmpActiveSkill(ActiveSkillArgsModel args)
        {
            var commandFormater = new SkillCommandModelFormatConverter();

            switch (args.StageNum)
            {
                case 44:
                    var tmpCheck = args.CheckActiveSkillCondition();
                    if (tmpCheck && !args.CharacterActiveSkillIsActivate[(int)UserPlayerRelativeType.Self][args.SkillIndex])
                    {
                        commandFormater.SkillTurnOnOffWithLineLight(true);
                    }
                    else if (!tmpCheck && args.CharacterActiveSkillIsActivate[(int)UserPlayerRelativeType.Self][args.SkillIndex])
                    {
                        commandFormater.SkillTurnOnOffWithLineLight(false);
                    }
                    break;
                case 2:
                    commandFormater.SkillAnimateStartPlay();
                    break;
                case 61:
                    commandFormater.PersonBloodControl(UserPlayerRelativeType.Opponent, 1, PersonBloodControlType.DirectDamage, 5);
                    commandFormater.PersonBloodControl(UserPlayerRelativeType.Opponent, 1, PersonBloodControlType.Heal, 2);
                    commandFormater.SkillTurnOnOffWithLineLight(false);
                    break;
                case 46:

                    break;
            }

            return commandFormater.Output();
        }
    }
}
