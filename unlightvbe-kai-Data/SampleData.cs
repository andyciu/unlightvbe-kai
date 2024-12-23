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
        private List<Player> Players { get; set; } = [];
        private static readonly Random Rnd = new();

        public class SampleCharacter(int num)
        {
            private int tmpActiveSkill2_DiceTotalTrue = 0;

            public unlightvbe_kai_core.Models.Character GetCharacter()
            {
                return new()
                {
                    Name = "Sample" + num.ToString(),
                    HP = 12,
                    ATK = 7,
                    DEF = 7,
                    VBEID = "SAMPLE" + num.ToString(),
                    EventColour = "325000",
                    LevelMain = "LV",
                    LevelNum = 5,
                    ActiveSkills = [Skill1, Skill2]
                };
            }
            public ActiveSkillModel Skill1 => new()
            {
                Function = TmpActiveSkill,
                Identifier = string.Format("VBEIDSAMPLE{0}001", num),
                Distance =
                        [
                            CommandPlayerDistanceType.Middle,
                            CommandPlayerDistanceType.Close,
                            CommandPlayerDistanceType.Long,
                        ],
                Phase = PhaseType.Move,
                StageNumber = [94, 2, 61, 46, 48],
                Cards = [],
                Name = "TmpActiveSkill"
            };

            public ActiveSkillModel Skill2 => new()
            {
                Function = TmpActiveSkill_2,
                Identifier = string.Format("VBEIDSAMPLE{0}002", num),
                Distance =
                        [
                            CommandPlayerDistanceType.Middle,
                            CommandPlayerDistanceType.Close,
                            CommandPlayerDistanceType.Long,
                        ],
                Phase = PhaseType.Attack,
                StageNumber = [42, 45, 10, 13, 47, 62, 20],
                Cards = [new()
                {
                    Scope = SkillCardConditionScopeType.Above,
                    CardType = ActionCardType.None,
                    Number = 1,
                }],
                Name = "TmpActiveSkill_2"
            };

            private List<SkillCommandModel> TmpActiveSkill(ActiveSkillArgsModel args)
            {
                var commandFormater = new SkillCommandModelFormatConverter();

                switch (args.StageNum)
                {
                    case 94:
                        args.CheckActiveSkillTurnOnOffStandardAction(commandFormater);
                        break;
                    case 2:
                        commandFormater.SkillAnimateStartPlay();
                        commandFormater.PersonMoveControl(CommandPlayerRelativeTwoVersionType.Self, NumberChangeRecordThreeVersionType.Addition, 3);
                        commandFormater.PersonMoveActionChange(CommandPlayerRelativeTwoVersionType.Self, PersonMoveActionType.BarMoveLeft);
                        commandFormater.BattleTurnControl(NumberChangeRecordTwoVersionType.Addition, 1);
                        commandFormater.SkillTurnOnOffWithLineLight(false);
                        break;
                    case 61:
                        commandFormater.PersonBloodControl(CommandPlayerRelativeTwoVersionType.Opponent, 1, PersonBloodControlType.DirectDamage, 5);
                        //commandFormater.PersonBloodControl(CommandPlayerRelativeTwoVersionType.Self, 1, PersonBloodControlType.Heal, 2);
                        commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Self, 1, "BUFFN00101", 100, 2);
                        commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Self, 1, "BUFFN00201", 10, 1);
                        commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Self, 1, "BUFFN00101", 999, 9);
                        //commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Opponent, 1, "BUFFN00102", 10, 2);
                        //commandFormater.PersonAddBuff(CommandPlayerRelativeTwoVersionType.Opponent, 1, "BUFFN00202", 10, 1);
                        commandFormater.SkillLineLightAnother(SkillType.ActiveSkill, 2, true);
                        commandFormater.SkillTurnOnOffAnother(SkillType.ActiveSkill, 2, true);
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

            private List<SkillCommandModel> TmpActiveSkill_2(ActiveSkillArgsModel args)
            {
                var commandFormater = new SkillCommandModelFormatConverter();

                switch (args.StageNum)
                {
                    case 42:
                        args.CheckActiveSkillTurnOnOffStandardAction(commandFormater);
                        break;
                    case 45:
                        commandFormater.EventTotalDiceChange(CommandPlayerRelativeTwoVersionType.Self, NumberChangeRecordSixVersionType.Addition, 200);
                        commandFormater.EventTotalDiceChange(CommandPlayerRelativeTwoVersionType.Opponent, NumberChangeRecordSixVersionType.Subtraction, 100);
                        break;
                    case 10:
                        commandFormater.BattleMoveControl(CommandPlayerDistanceType.Close);
                        commandFormater.PersonTotalDiceControl(CommandPlayerRelativeTwoVersionType.Self, NumberChangeRecordSixVersionType.Division_Floor, 2);
                        commandFormater.SkillAnimateStartPlay();
                        break;
                    case 13:
                        this.tmpActiveSkill2_DiceTotalTrue = 0;
                        for (int i = 0; i < 10; i++)
                        {
                            commandFormater.BattleStartDice();
                        }
                        break;
                    case 47:
                        var messageModel47 = (StageMessageModel_47)StageMessageModelFactory.GetModel<StageMessageModel_47>(args.StageMessage)!;
                        if (messageModel47.TriggerPlayer == CommandPlayerRelativeThreeVersionType.Opponent)
                        {
                            commandFormater.EventMoveActionOff();
                        }
                        break;
                    case 62:
                        var messageModel62 = (StageMessageModel_62)StageMessageModelFactory.GetModel<StageMessageModel_62>(args.StageMessage)!;
                        this.tmpActiveSkill2_DiceTotalTrue += messageModel62.DiceTrueTotal;
                        break;
                    case 20:
                        if (args.Phase == PhaseType.Attack)
                        {
                            commandFormater.AttackTrueDiceControl(NumberChangeRecordThreeVersionType.Addition, this.tmpActiveSkill2_DiceTotalTrue);
                            commandFormater.SkillTurnOnOffWithLineLight(false);
                        }
                        break;
                }

                return commandFormater.Output();
            }
        }

        public SampleData()
        {
            Players.Add(new()
            {
                Name = "AAA",
                PlayerId = 1,
                Deck = new()
                {
                    Id = 1,
                    Name = "D1",
                    Deck_Subs = Enumerable.Range(1, 3).Select(i => new Deck_Sub
                    {
                        Character = new SampleCharacter(i).GetCharacter(),
                        EventCards = GetCardList_Event()
                    }).ToList()
                }
            });

            Players.Add(new()
            {
                Name = "BBB",
                PlayerId = 2,
                Deck = new()
                {
                    Id = 2,
                    Name = "D2",
                    Deck_Subs = Enumerable.Range(4, 3).Select(i => new Deck_Sub
                    {
                        Character = new SampleCharacter(i).GetCharacter(),
                        EventCards = GetCardList_Event()
                    }).ToList()
                }
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
    }
}
