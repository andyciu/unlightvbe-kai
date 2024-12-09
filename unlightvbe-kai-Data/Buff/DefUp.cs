using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.Skill;
using unlightvbe_kai_core.Models.SkillArgs;

namespace unlightvbe_kai_Data.Buff
{
    public class DefUp
    {
        public static readonly BuffSkillModel BuffSkillObj = new()
        {
            Function = BuffFuc,
            Identifier = "BUFFN00201",
            StageNumber = [45, 53],
            Name = "DEF+"
        };
        private static List<SkillCommandModel> BuffFuc(BuffArgsModel args)
        {
            var commandFormater = new SkillCommandModelFormatConverter();

            switch (args.StageNum)
            {
                case 45:
                    if (args.CharacterIsOnField && args.Phase == PhaseType.Defense)
                    {
                        commandFormater.EventPersonAbilityDiceChange(
                            CommandPlayerRelativeTwoVersionType.Self, NumberChangeRecordThreeVersionType.Addition, args.BuffValue);
                    }
                    break;
                case 53:
                    if (args.CharacterIsOnField) commandFormater.BuffTurnEnd();
                    break;
            }

            return commandFormater.Output();
        }
    }
}
