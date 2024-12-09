using System.Reflection;
using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.Skill;
using unlightvbe_kai_core.Models.SkillArgs;
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
            ActiveSkillModel skill1 = new()
            {
                Function = TmpActiveSkill,
                Identifier = "VBEID001",
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
            Characters.Add(new()
            {
                Name = "Sheri",
                HP = 14,
                ATK = 4,
                DEF = 5,
                VBEID = "N00105",
                EventColour = "776000",
                LevelMain = "LV",
                LevelNum = 5,
                ActiveSkills = [skill1]
            });

            //Characters.Add(Ria.GetCharacter());
            //Characters.Add(Evarist.GetCharacter());

            for (int i = 0; i < 3; i++)
            {
                Deck_Subs.Add(new()
                {
                    Character = Characters[i],
                    EventCards = GetCardList_Event()
                });
            }

            for (int i = 0; i < 2; i++)
            {
                Decks.Add(new()
                {
                    Id = i,
                    Name = "D" + i.ToString(),
                    Deck_Subs = [.. Deck_Subs]
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
                    Identifier = "ASG01"
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.DEF,
                    LowerNum = Rnd.Next(1, 7),
                    Identifier = "ASD01"
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Gun,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.DEF,
                    LowerNum = Rnd.Next(1, 7),
                    Identifier = "AGD01"
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Sword,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.SPE,
                    LowerNum = Rnd.Next(1, 7),
                    Identifier = "ASS01"
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.ATK_Gun,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.SPE,
                    LowerNum = Rnd.Next(1, 7),
                    Identifier = "AGS01"
                });
                cards.Add(new ActionCard
                {
                    UpperType = ActionCardType.MOV,
                    UpperNum = Rnd.Next(1, 7),
                    LowerType = ActionCardType.SPE,
                    LowerNum = Rnd.Next(1, 7),
                    Identifier = "MS01"
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
                    Identifier = "RRE01"
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

        public static List<BuffSkillModel> GetBuffList()
        {
            List<BuffSkillModel> buffList = [];

            var typeList = (from t in Assembly.GetExecutingAssembly().GetTypes()
                            where t.IsClass && t.Namespace == "unlightvbe_kai_Data.Buff"
                            select t).ToList();

            foreach (var item in typeList)
            {
                var tmpfield = item.GetField("BuffSkillObj");
                if (tmpfield != null)
                {
                    buffList.Add((BuffSkillModel)tmpfield.GetValue(null)!);
                }
            }

            return buffList;
        }

        public static Dictionary<string, string> GetBuffNameDict()
        {
            Dictionary<string, string> result = [];
            var typeList = (from t in Assembly.GetExecutingAssembly().GetTypes()
                            where t.IsClass && t.Namespace == "unlightvbe_kai_Data.Buff"
                            select t).ToList();

            foreach (var item in typeList)
            {
                var tmpfield = item.GetField("BuffSkillObj");
                if (tmpfield != null)
                {
                    var buffitem = (BuffSkillModel)tmpfield.GetValue(null)!;
                    result.Add(buffitem.Identifier, buffitem.Name);
                }
            }

            return result;
        }

        private static List<SkillCommandModel> TmpActiveSkill(ActiveSkillArgsModel args)
        {
            var commandFormater = new SkillCommandModelFormatConverter();

            switch (args.StageNum)
            {
                case 94:
                    args.CheckActiveSkillTurnOnOffStandardAction(commandFormater);
                    break;
                case 2:
                    commandFormater.SkillAnimateStartPlay();
                    break;
                case 61:
                    commandFormater.PersonBloodControl(CommandPlayerRelativeTwoVersionType.Opponent, 1, PersonBloodControlType.DirectDamage, 5);
                    //commandFormater.PersonBloodControl(CommandPlayerRelativeTwoVersionType.Self, 1, PersonBloodControlType.Heal, 2);
                    commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Self, 1, "BUFFN00101", 100, 2);
                    commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Self, 1, "BUFFN00201", 10, 1);
                    commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Self, 1, "BUFFN00101", 999, 9);
                    //commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Opponent, 1, "BUFFN00102", 10, 2);
                    //commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Opponent, 1, "BUFFN00202", 10, 1);
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
