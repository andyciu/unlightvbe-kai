using unlightvbe_kai_core;
using unlightvbe_kai_core.Enum;
using unlightvbe_kai_core.Enum.SkillCommand;
using unlightvbe_kai_core.Models;
using unlightvbe_kai_core.Models.Skill;
using unlightvbe_kai_core.Models.SkillArgs;

namespace unlightvbe_kai_Data.Character
{
    public class Evarist
    {
        public unlightvbe_kai_core.Models.Character GetCharacter()
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

        private ActiveSkillModel ActiveSkillObj_1 => new()
        {
            Function = ActiveSkillFuc_1,
            Identifier = "PNAKN01101",
            Distance =
            [
                CommandPlayerDistanceType.Middle,
                CommandPlayerDistanceType.Close,
                CommandPlayerDistanceType.Long,
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

        private List<SkillCommandModel> ActiveSkillFuc_1(ActiveSkillArgsModel args)
        {
            var commandFormater = new SkillCommandModelFormatConverter();

            switch (args.StageNum)
            {
                case 42:
                    args.CheckActiveSkillTurnOnOffStandardAction(commandFormater);
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
