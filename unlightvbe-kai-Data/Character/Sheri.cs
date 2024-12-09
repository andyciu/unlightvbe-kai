using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.Skill;
using unlightvbe_kai_core.Models.SkillArgs;

namespace unlightvbe_kai_Data.Character
{
    public class Sheri
    {
        public static unlightvbe_kai_core.Models.Character GetCharacter()
        {
            return new()
            {
                Name = "Sheri",
                HP = 14,
                ATK = 4,
                DEF = 5,
                VBEID = "N00105",
                EventColour = "776000",
                LevelMain = "LV",
                LevelNum = 5,
                ActiveSkills = [ActiveSkillObj_1]
            };
        }

        private static readonly ActiveSkillModel ActiveSkillObj_1 = new()
        {
            Function = ActiveSkillFuc_1,
            Identifier = "PNAKN00101",
            Distance =
            [
                PlayerDistanceType.Middle,
                PlayerDistanceType.Close,
                PlayerDistanceType.Long,
            ],
            Phase = PhaseType.Attack,
            StageNumber = [42, 45, 11, 61],
            Cards = [ new()
            {
                Scope = SkillCardConditionScopeType.Above,
                CardType = ActionCardType.SPE,
                Number = 1
            }],
            Name = "自殺傾向"
        };

        private static List<SkillCommandModel> ActiveSkillFuc_1(ActiveSkillArgsModel args)
        {
            var commandFormater = new SkillCommandModelFormatConverter();

            switch (args.StageNum)
            {
                case 42:
                    args.CheckActiveSkillTurnOnOffStandardAction(commandFormater);
                    break;
                case 45:
                    commandFormater.EventTotalDiceChange(CommandPlayerRelativeTwoVersionType.Self, NumberChangeRecordSixVersionType.Addition, args.ActionCardTotal[(int)UserPlayerRelativeType.Self][ActionCardType.SPE] * 5);
                    break;
                case 11:
                    commandFormater.SkillAnimateStartPlay();
                    commandFormater.SkillTurnOnOffWithLineLight(false);
                    break;
                case 61:
                    commandFormater.PersonBloodControl(CommandPlayerRelativeTwoVersionType.Self, 1, PersonBloodControlType.DirectDamage, args.ActionCardTotal[(int)UserPlayerRelativeType.Self][ActionCardType.SPE]);
                    break;
            }

            return commandFormater.Output();
        }
    }
}
