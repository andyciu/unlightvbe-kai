using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.Skill;
using unlightvbe_kai_core.Models.SkillArgs;

namespace unlightvbe_kai_Data.Buff
{
    public class AtkUp
    {
        public static readonly BuffSkillModel BuffSkillObj = new()
        {
            Function = BuffFuc,
            Identifier = "BUFFN00101",
            StageNumber = [45, 53],
            Name = "ATK+"
        };
        private static List<SkillCommandModel> BuffFuc(BuffArgsModel args)
        {
            var commandFormater = new SkillCommandModelFormatConverter();

            switch (args.StageNum)
            {
                case 45:
                    if (args.CharacterIsOnField && args.Phase == PhaseType.Attack &&
                        ((args.PlayerDistance == PlayerDistanceType.Close && args.ActionCardTotal[(int)UserPlayerRelativeType.Self][ActionCardType.ATK_Sword] > 0) ||
                        (args.PlayerDistance != PlayerDistanceType.Close && args.ActionCardTotal[(int)UserPlayerRelativeType.Self][ActionCardType.ATK_Gun] > 0)))
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
