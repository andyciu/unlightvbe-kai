using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.StageMessage;
using unlightvbe_kai_Data.Character;

namespace unlightvbe_kai_Data
{
    public class SampleData
    {
        public List<unlightvbe_kai_core.Models.Character> Characters { get; set; } = [];
        public List<Deck_Sub> Deck_Subs { get; set; } = [];
        public List<Deck> Decks { get; set; } = [];
        public List<Player> Players { get; set; } = [];
        public static readonly Random Rnd = new();
        public SampleData()
        {
            Skill<ActiveSkill> skill1 = new(TmpActiveSkill, "VBEID001")
            {
                Distance =
                        [
                            PlayerDistanceType.Middle,
                            PlayerDistanceType.Close,
                            PlayerDistanceType.Long,
                        ],
                Phase = PhaseType.Move,
                StageNumber = [94, 2, 61, 46, 48],
                Cards = [],
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
                ActiveSkills = [skill1]
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
                ActiveSkills = [skill1]
            });

            //Characters.Add(Ria.GetCharacter());
            //Characters.Add(Evarist.GetCharacter());

            for (int i = 0; i < 2; i++)
            {
                Deck_Subs.Add(new()
                {
                    Character = Characters[i],
                    EventCards = GetCardList_Event()
                });

                Decks.Add(new()
                {
                    Id = i,
                    Name = "D" + i.ToString(),
                    Deck_Subs = [Deck_Subs[i]]
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

        public static List<SkillCommandModel> TmpActiveSkill(ActiveSkillArgsModel args)
        {
            var commandFormater = new SkillCommandModelFormatConverter();

            switch (args.StageNum)
            {
                case 94:
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
                    commandFormater.PersonBloodControl(CommandPlayerRelativeTwoVersionType.Opponent, 1, PersonBloodControlType.DirectDamage, 5);
                    commandFormater.PersonBloodControl(CommandPlayerRelativeTwoVersionType.Opponent, 1, PersonBloodControlType.Heal, 2);
                    commandFormater.SkillTurnOnOffWithLineLight(false);
                    break;
                case 46:
                    var messageModel46 = (StageMessageModel_46)StageMessageModelFactory.GetModel<StageMessageModel_46>(args.StageMessage)!;
                    if (messageModel46.TargetPlayer == CommandPlayerRelativeTwoVersionType.Self)
                    {
                        commandFormater.EventBloodActionChange(NumberChangeRecordThreeVersionType.Subtraction, 1);
                        commandFormater.EventBloodReflection(CommandPlayerRelativeTwoVersionType.Opponent, 1, 1);
                    }
                    break;
                case 48:
                    var messageModel48 = (StageMessageModel_48)StageMessageModelFactory.GetModel<StageMessageModel_48>(args.StageMessage)!;
                    if (messageModel48.TargetPlayer == CommandPlayerRelativeTwoVersionType.Opponent)
                    {
                        commandFormater.EventHealActionOff();
                    }
                    break;
            }

            return commandFormater.Output();
        }
    }
}
