using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;

namespace unlightvbe_kai_Data.Character
{
    public class Evarist
    {
        public static unlightvbe_kai_core.Models.Character GetCharacter()
        {
            return new()
            {
                Name = "Evarist",
                HP = 12,
                ATK = 7,
                DEF = 7,
                VBEID = "N01105",
                EventColour = "325000",
                LevelMain = "LV",
                LevelNum = 5,
                ActiveSkills = [ActiveSkillObj_1]
            };
        }

        private static readonly Skill<ActiveSkill> ActiveSkillObj_1 = new(ActiveSkillFuc_1, "PNAKN01101")
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
                CardType = ActionCardType.ATK_Gun,
                Number = 2
            }],
            Name = "精密射擊"
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
                    commandFormater.EventTotalDiceChange(CommandPlayerRelativeTwoVersionType.Self, NumberChangeRecordSixVersionType.Addition, 4);
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
