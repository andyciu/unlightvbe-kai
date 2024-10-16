using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_Data.Character
{
    public class Ria
    {
        public static unlightvbe_kai_core.Models.Character GetCharacter()
        {
            return new()
            {
                Name = "Ria",
                HP = 9,
                ATK = 5,
                DEF = 10,
                VBEID = "S00105",
                EventColour = "357000",
                LevelMain = "LV",
                LevelNum = 5,
                ActiveSkills = [ActiveSkillObj_1]
            };
        }

        private static readonly Skill<ActiveSkill> ActiveSkillObj_1 = new(ActiveSkillFuc_1, "PNAKS00101")
        {
            Distance =
            [
                PlayerDistanceType.Middle,
                PlayerDistanceType.Close,
                PlayerDistanceType.Long,
            ],
            Phase = PhaseType.Attack,
            StageNumber = [42, 45, 11],
            Cards = [ new()
            {
                Scope = SkillCardConditionScopeType.Above,
                CardType = ActionCardType.ATK_Sword,
                Number = 4
            }],
            Name = "輪旋曲-琉璃色的微風"
        };

        public static List<SkillCommandModel> ActiveSkillFuc_1(ActiveSkillArgsModel args)
        {
            var commandFormater = new SkillCommandModelFormatConverter();

            switch (args.StageNum)
            {
                case 42:
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
                case 45:
                    commandFormater.EventTotalDiceChange(CommandPlayerRelativeTwoVersionType.Self, NumberChangeRecordSixVersionType.Addition, 6);
                    commandFormater.EventTotalDiceChange(CommandPlayerRelativeTwoVersionType.Opponent, NumberChangeRecordSixVersionType.Subtraction, 3);
                    break;
                case 11:
                    commandFormater.SkillAnimateStartPlay();
                    commandFormater.SkillTurnOnOffWithLineLight(false);
                    break;
            }

            return commandFormater.Output();
        }
    }
}
